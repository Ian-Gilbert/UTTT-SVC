using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UtttApi.ObjectModel.Interfaces;
using UtttApi.ObjectModel.Models;

namespace UtttApi.WebApi.Controllers
{
    /// <summary>
    /// Controls the main game funtionality
    /// </summary>
    [ApiController]
    [Route("rest/[controller]")]
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
        [ProducesResponseType(typeof(GameObject), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(string id)
        {
            GameObject game = await _unitOfWork.Game.SelectAsync(id);

            if (game == null)
            {
                return NotFound($"Could not find the game with id {id}");
            }

            return Ok(await _unitOfWork.Game.SelectAsync(id));
        }

        /// <summary>
        /// Create a new game
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(GameObject), StatusCodes.Status202Accepted)]
        public async Task<IActionResult> PostAsync()
        {
            return Accepted(await _unitOfWork.Game.InsertAsync(new GameObject()));
        }

        /// <summary>
        /// Make a move on a game by its ID number
        /// </summary>
        /// <param name="id"></param>
        /// <param name="player"></param>
        /// <param name="lb_index"></param>
        /// <param name="move"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(GameObject), StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutAsync(string id, [FromBody] MoveObject move)
        {
            GameObject game = await _unitOfWork.Game.SelectAsync(id);

            if (game == null)
            {
                return NotFound($"Could not find the game with id {id}");
            }

            if (game.IsValidMove(move))
            {
                if (game.CheckPlayerMove(move))
                {
                    game.MakeMove(move);
                    game.UpdateGameStatus();
                    await _unitOfWork.Game.UpdateAsync(game);
                    return Accepted(game);
                }

                return BadRequest($"It is not player {move.Player}'s turn.");
            }

            return BadRequest($"The move ({move.LbIndex}, {move.MarkIndex}) is not valid for player {move.Player}.");
        }

        /// <summary>
        /// Delete a game by its ID number
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            await _unitOfWork.Game.DeleteAsync(id);
            return Ok();
        }
    }
}