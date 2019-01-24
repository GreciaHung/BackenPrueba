using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using API.Core;
using API.Repositories;

namespace API.Controllers
{
    [Authorize]
    public class PoliciesController : ApiController
    {
        private PolicyRepository policyRepository = new PolicyRepository();

        // GET: api/Policies
        public IQueryable<Policy> GetPolicies()
        {
            return policyRepository.GetAll();
        }

        // GET: api/Policies/5
        [ResponseType(typeof(Policy))]
        public async Task<IHttpActionResult> GetPolicy(int id)
        {
            Policy policy = await policyRepository.GetOneAsync(id);
            if (policy == null)
            {
                return NotFound();
            }

            return Ok(policy);
        }

        // PUT: api/Policies/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPolicy(int id, Policy policy)
        {
            if (id != policy.Id)
            {
                return BadRequest();
            }

            var current = await policyRepository.GetOneAsync(id);
            current.TypeRisk = policy.TypeRisk;
            current.Price = policy.Price;
            current.PercentageCoverage = policy.PercentageCoverage;
            current.Description = policy.Description;

            policyRepository.Edit(current);

            try
            {
                await policyRepository.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!policyRepository.PolicyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Policies
        [ResponseType(typeof(Policy))]
        public async Task<IHttpActionResult> PostPolicy(Policy policy)
        {
            policyRepository.Add(policy);
            await policyRepository.SaveAsync();

            return CreatedAtRoute("DefaultApi", new { id = policy.Id }, policy);
        }

        // DELETE: api/Policies/5
        [ResponseType(typeof(Policy))]
        public async Task<IHttpActionResult> DeletePolicy(int id)
        {
            Policy policy = await policyRepository.GetOneAsync(id);

            if(DateTimeOffset.Now < policy.EndDate)
            {
                ModelState.AddModelError("msg", "No se pueden eliminar pólizas que estén en vigencia.");
                return BadRequest(ModelState);
            }

            policyRepository.Delete(policy);
            await policyRepository.SaveAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                policyRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}