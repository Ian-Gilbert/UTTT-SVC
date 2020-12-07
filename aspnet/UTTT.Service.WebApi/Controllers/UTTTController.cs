using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UTTT.Service.ObjectModel.Models;

namespace UTTT.Service.WebApi.Controllers
{
    [ApiController]
    [Route("rest/uttt")]
    public class UTTTController : ControllerBase
    {
        /// <summary>
        /// Get a game by it's ID number
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GameObject), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(int id)
        {
            return Ok(new GameObject());
        }

        /// <summary>
        /// Create a new game
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(GameObject), StatusCodes.Status202Accepted)]
        public async Task<IActionResult> PostAsync()
        {
            return Accepted(new GameObject());
        }

        /// <summary>
        /// Make a move
        /// </summary>
        /// <param name="id"></param>
        /// <param name="player"></param>
        /// <param name="lb_index"></param>
        /// <param name="move"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(GameObject), StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutAsync(int id, [FromBody] int player, [FromBody] int lb_index, [FromBody] int move)
        {
            return Accepted(new GameObject());
        }

        /// <summary>
        /// Delete a game
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok();
        }
    }
}