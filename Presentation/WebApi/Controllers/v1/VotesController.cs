using Application.Features.Votes.Commands.Create;
using Application.Features.Votes.Queries.GetById;
using Application.Features.Votes.Queries.GetPagedList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


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
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await Mediator.Send(new GetVoteByIdQuery { Id = id }));
        }


        /// <summary>
        /// Creates a Vote.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>A newly created Vote</returns>
        /// <response code="201">Returns the newly created command</response>
        /// <response code="400">If the command is null</response>            
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(CreateVoteCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
