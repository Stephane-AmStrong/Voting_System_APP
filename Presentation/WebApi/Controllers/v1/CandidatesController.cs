using Application.Features.Candidates.Commands.AddToCategory;
using Application.Features.Candidates.Commands.Create;
using Application.Features.Candidates.Commands.Delete;
using Application.Features.Candidates.Commands.Update;
using Application.Features.Candidates.Queries.GetById;
using Application.Features.Candidates.Queries.GetPagedList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;


namespace WebApi.Controllers.v1
{
    [Authorize]
    [ApiVersion("1.0")]
    public class CandidatesController : BaseApiController
    {
        //readonly IDiagnosticContext _diagnosticContext;
        public CandidatesController()
        {
        }


        /// <summary>
        /// return candidates that matche the criteria
        /// </summary>
        /// <param name="candidatesQuery"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "candidate.read.policy")]
        public async Task<IActionResult> Get([FromQuery] GetCandidatesQuery candidatesQuery)
        {
            var candidates = await Mediator.Send(candidatesQuery);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(candidates.MetaData));
            return Ok(candidates.PagedList);
        }


        /// <summary>
        /// Retreives a specific Candidate.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Policy = "candidate.read.policy")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await Mediator.Send(new GetCandidateByIdQuery { Id = id }));
        }


        /// <summary>
        /// Creates a Candidate.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>A newly created Candidate</returns>
        /// <response code="201">Returns the newly created command</response>
        /// <response code="400">If the command is null</response>            
        [HttpPost]
        [Authorize(Policy = "candidate.write.policy")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(CreateCandidateCommand command)
        {
            return Ok(await Mediator.Send(command));
        }


        /// <summary>
        /// Add Candidate to Category.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>An update of the Candidate</returns>
        /// <response code="201">Returns the newly created command</response>
        /// <response code="400">If the command is null</response>            
        [HttpPut("add-candidate-to-category")]
        [Authorize(Policy = "candidate.write.policy")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddToCategory(AddCandidateToCategoryCommand command)
        {
            return Ok(await Mediator.Send(command));
        }


        /// <summary>
        /// Update a specific Candidate.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Policy = "candidate.write.policy")]
        public async Task<IActionResult> Put(Guid id, UpdateCandidateCommand command)
        {
            command.Id = id;
            return Ok(await Mediator.Send(command));
        }


        /// <summary>
        /// Deletes a specific Candidate.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Policy = "candidate.manage.policy")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await Mediator.Send(new DeleteCandidateCommand { Id = id });
            return NoContent();
        }
    }
}
