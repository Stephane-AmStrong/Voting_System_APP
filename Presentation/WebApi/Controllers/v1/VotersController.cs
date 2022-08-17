using Application.Features.Voters.Commands.ChangeAge;
using Application.Features.Voters.Commands.Create;
using Application.Features.Voters.Commands.Delete;
using Application.Features.Voters.Commands.Update;
using Application.Features.Voters.Queries.GetById;
using Application.Features.Voters.Queries.GetPagedList;
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
    public class VotersController : BaseApiController
    {
        //readonly IDiagnosticContext _diagnosticContext;
        public VotersController()
        {
        }


        /// <summary>
        /// return voters that matche the criteria
        /// </summary>
        /// <param name="votersQuery"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "voter.read.policy")]
        public async Task<IActionResult> Get([FromQuery] GetVotersQuery votersQuery)
        {
            var voters = await Mediator.Send(votersQuery);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(voters.MetaData));
            return Ok(voters.PagedList);
        }


        /// <summary>
        /// Retreives a specific Voter.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Policy = "voter.read.policy")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await Mediator.Send(new GetVoterByIdQuery { Id = id }));
        }


        /// <summary>
        /// Creates a Voter.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>A newly created Voter</returns>
        /// <response code="201">Returns the newly created command</response>
        /// <response code="400">If the command is null</response>            
        [HttpPost]
        [Authorize(Policy = "voter.write.policy")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(CreateVoterCommand command)
        {
            command.Origin = Request.Headers["origin"];
            return Ok(await Mediator.Send(command));
        }


        /// <summary>
        /// Update a specific Voter.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Policy = "voter.write.policy")]
        public async Task<IActionResult> Put(string id, UpdateVoterCommand command)
        {
            command.Id = id;
            command.Origin = Request.Headers["origin"];
            return Ok(await Mediator.Send(command));
        }


        /// <summary>
        /// Change the birthday of a specific Voter.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("change-age/{id}")]
        [Authorize(Policy = "voter.write.policy")]
        public async Task<IActionResult> ChangeAge(string id, ChangeAgeVoterCommand command)
        {
            command.Id = id;
            command.Origin = Request.Headers["origin"];
            return Ok(await Mediator.Send(command));
        }


        /// <summary>
        /// Deletes a specific Voter.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Policy = "voter.manage.policy")]
        public async Task<IActionResult> Delete(string id)
        {
            await Mediator.Send(new DeleteVoterCommand { Id = id });
            return NoContent();
        }
    }
}
