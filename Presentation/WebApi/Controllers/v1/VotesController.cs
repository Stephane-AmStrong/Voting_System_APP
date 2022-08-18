using Application.Features.Votes.Commands.Create;
using Application.Features.Votes.Queries.GetById;
using Application.Features.Votes.Queries.GetNumberOfVotesForACandidate;
using Application.Features.Votes.Queries.GetPagedList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace WebApi.Controllers.v1
{
    [Authorize]
    [ApiVersion("1.0")]
    public class VotesController : BaseApiController
    {
        //readonly IDiagnosticContext _diagnosticContext;
        public VotesController()
        {
        }


        /// <summary>
        /// return votes that matche the criteria
        /// </summary>
        /// <param name="votesQuery"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "vote.read.policy")]
        public async Task<IActionResult> Get([FromQuery] GetVotesQuery votesQuery)
        {
            var votes = await Mediator.Send(votesQuery);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(votes.MetaData));
            return Ok(votes.PagedList);
        }


        /// <summary>
        /// Retreives a specific Vote.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Policy = "vote.read.policy")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await Mediator.Send(new GetVoteByIdQuery { Id = id }));
        }


        /// <summary>
        /// Retreives the Number of votes for candidate with the specified.
        /// </summary>
        /// <param name="CandidateId"></param>
        /// <returns></returns>
        [HttpGet("number-of-votes-for-candidate/{CandidateId}")]
        [Authorize(Policy = "vote.read.policy")]
        public async Task<IActionResult> GetNumberOfVotesForACandidate(string CandidateId)
        {
            return Ok(await Mediator.Send(new GetNumberOfVotesForACandidateQuery { CandidateId = CandidateId }));
        }


        /// <summary>
        /// Creates a Vote.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>A newly created Vote</returns>
        /// <response code="201">Returns the newly created command</response>
        /// <response code="400">If the command is null</response>            
        [HttpPost]
        [Authorize(Policy = "vote.write.policy")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(CreateVoteCommand command)
        {
            command.VoterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(await Mediator.Send(command));
        }
    }
}
