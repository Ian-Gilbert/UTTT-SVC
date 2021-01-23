using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UtttApi.DataService.Interfaces;
using UtttApi.ObjectModel.Models;

namespace UtttApi.WebApi.Controllers
{
    /// <summary>
    /// Controls the main game funtionality
    /// </summary>
    [ApiController]
    [Route("rest/uttt/[controller]")]
    public class UtttController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public UtttController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Get a game by its ID number
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UtttObject), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(string id) =>
            Ok(await _unitOfWork.Game.SelectAsync(id));

        /// <summary>
        /// Create a new game and return the new Id
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        public async Task<IActionResult> Post()
        {
            var game = await _unitOfWork.Game.InsertAsync(new UtttObject());
            return CreatedAtAction(
                nameof(Get),
                new { id = game.Id },
                game
            );
        }

        /// <summary>
        /// Make a move on a game by its ID number
        /// </summary>
        /// <param name="id"></param>
        /// <param name="player"></param>
        /// <param name="lbIndex"></param>
        /// <param name="markIndex"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UtttObject), StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(string id, Move move)
        {
            UtttObject game = await _unitOfWork.Game.SelectAsync(id);
            game.MakeMove(move);
            await _unitOfWork.Game.UpdateAsync(game);
            return Accepted(game);
        }

        /// <summary>
        /// Delete a game by its ID number
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            await _unitOfWork.Game.DeleteAsync(id);
            return NoContent();
        }
    }
}