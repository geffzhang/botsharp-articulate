using BotSharp.Platform.Articulate.Models;
using BotSharp.Platform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BotSharp.Platform.Articulate.Controllers
{
    [Route("[controller]")]
    public class TrainController : ControllerBase
    {
        private ArticulateAi<AgentModel> builder;

        public TrainController(IConfiguration configuration)
        {
            builder = new ArticulateAi<AgentModel>();
            builder.PlatformConfig = configuration.GetSection("ArticulateAi");
        }

        [HttpGet("/agent/{agentId}/train")]
        public async Task<AgentModel> TrainAgent([FromRoute] string agentId)
        {
            var agent = builder.GetAgentById(agentId);

            var corpus = builder.ExtractorCorpus(agent);

            await builder.Train(agent, corpus, new BotTrainOptions { });

            agent.Status = "Ready";

            builder.SaveAgent(agent);

            return agent;
        }
    }
}
