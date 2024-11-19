using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; 
using Blood_Bank_Management.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Drawing;

namespace Blood_Bank_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonorController : Controller 
    {
        // static (default data )
        static List<Donor> donorList = new List<Donor>
        {
            new Donor{Id=1,DonorName = "Srijith",Age = 20,BloodType="O+",ContactInfo="123123",Quantity=500,CollectionDate=DateTime.Now,ExpirationDate = (DateTime.Now).AddDays(42),Status = "Available"}
        };


        //create data  with all conditions :) validations
        [HttpPost]
        public ActionResult<IEnumerable<Donor>> Add(Donor d)
        {
            if (string.IsNullOrEmpty(d.DonorName))
            {
                return BadRequest("Donor name is required.");
            }

            if (d.Age <= 18 || d.Age > 75)
            {
                return BadRequest("Invalid age. Age should be between 18 and 75.");
            }

            if (string.IsNullOrEmpty(d.BloodType) ||
                    !new HashSet<string> { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" }.Contains(d.BloodType.ToUpper()))
            {
                return BadRequest("Invalid blood type. Please provide a valid blood type (e.g., A+, O-, B+).");
            }

            if (string.IsNullOrEmpty(d.ContactInfo) || !d.ContactInfo.All(char.IsDigit) || d.ContactInfo.Length != 10)
            {
                return BadRequest("Contact info must be a valid 10-digit phone number.");
            }

            if(d.Quantity < 200)
            {
                return BadRequest("Please enter more than 200ml.");
            }

            d.ExpirationDate = d.CollectionDate.AddDays(42);
            var validStatuses = new HashSet<string> { "available", "requested", "expired" };
            if (string.IsNullOrEmpty(d.Status) || !validStatuses.Contains(d.Status.ToLower()))
            {
                return BadRequest("Invalid status. Please provide a valid status (Available, Requested, or Expired).");
            }
            if ((d.ExpirationDate - d.CollectionDate).Days > 42)
            {
                d.Status = "Expired";
            }

            d.Id = donorList.Any() ? donorList.Max(x => x.Id) + 1 : 1;

            donorList.Add(d);

            return CreatedAtAction( nameof(GetAll),donorList);
        } 


        // get all data 
        [HttpGet("get all")]
        public ActionResult<IEnumerable<Donor>> GetAll()
        {
            if (!donorList.Any()) { return BadRequest("The list is empty"); }
            return donorList;
        }


        //get donor by id 
        [HttpGet("{id}")]
        public ActionResult<Donor> GetById(int id)
        {
            var res = donorList.Find(x => x.Id == id);
            if (res == null) { return BadRequest("ID not found , Try Again...."); }
            return res;
        }


        //update the donors list
        [HttpPut("{id}")]
        public ActionResult Update(int id,Donor d)
        {
            var res = donorList.Find(x=>x.Id == id);
            if (res == null) { return NotFound(); }
            var validStatuses = new HashSet<string> { "available", "requested", "expired" };
            if (string.IsNullOrEmpty(d.Status) || !validStatuses.Contains(d.Status.ToLower()))
            {
                return BadRequest("Invalid status. Please provide a valid status (Available, Requested, or Expired).");
            }
            res.DonorName = d.DonorName;
            res.Age = d.Age;
            res.BloodType = d.BloodType;
            res.ContactInfo = d.ContactInfo;
            res.Quantity = d.Quantity;
            res.CollectionDate = d.CollectionDate;
            res.ExpirationDate = d.ExpirationDate;
            res.Status = d.Status;

            return CreatedAtAction(nameof(GetAll), donorList);
        }



        //Delete
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var res = donorList.Find(x=>x.Id==id);
            if (res == null) { return NotFound(); }
            donorList.Remove(res);
            return CreatedAtAction(nameof(GetAll),donorList);
        }


        //pagination -> no of donors per page
        [HttpGet("page")]
        public ActionResult<IEnumerable<Donor>> GetPage(int page=1,int size = 10)
        {
            var res = donorList.Skip((page-1)*size).Take(size).ToList();
            return res;
        }


        //Search Functionality
        [HttpGet("blood")]
        public ActionResult<IEnumerable<Donor>> BloodSearch(string blood)
        {
            var res = donorList.FindAll(x => x.BloodType.ToUpper() == blood.ToUpper());
            if (res == null) { return NotFound(); };
            return res;
        }

        //Search Functionality for status
        [HttpGet("status")]
        public ActionResult<IEnumerable<Donor>> StatusSearch(string status)
        {
            var validStatuses = new HashSet<string> { "available", "requested", "expired" };
            if (string.IsNullOrEmpty(status) || !validStatuses.Contains(status.ToLower()))
            {
                return BadRequest("Invalid status. Please provide a valid status (Available, Requested, or Expired).");
            }
            var res = donorList.FindAll(x=>x.Status.ToUpper() == status.ToUpper());
            if (res == null) { return NotFound(); }
            return res;
        }

        //search based on donor name
        [HttpGet("name")]
        public ActionResult<IEnumerable<Donor>> NameSearch(string name)
        {
            var res = donorList.FindAll(x => x.DonorName.ToUpper().Contains(name.ToUpper()));
            if (res == null) { return NotFound(); };
            return res;
        }



        /***  BONUS  ***/

        //sort based on bloodtype & collection date
        [HttpGet("sort")]
        public ActionResult<IEnumerable<Donor>> SortProd(string sortby = "Blood", string sortorder = "asc")
        {
            var res = donorList.AsQueryable();

            if (sortby.ToLower() == "blood")
            {
                res = sortorder.ToLower() == "asc" ? res.OrderBy(i => i.BloodType) : res.OrderByDescending(i => i.BloodType);
            }
            else if (sortby.ToLower() == "datetime")
            {
                res = sortorder.ToLower() == "asc" ? res.OrderBy(i => i.CollectionDate) : res.OrderByDescending(i => i.CollectionDate);
            }
            return res.ToList();
        }

        //filter on blood and status 
        [HttpGet("blood,status")]
        public ActionResult<IEnumerable<Donor>> BloodSearch(string blood,string status)
        {
            var validStatuses = new HashSet<string> { "available", "requested", "expired" };
            if (string.IsNullOrEmpty(status) || !validStatuses.Contains(status.ToLower()))
            {
                return BadRequest("Invalid status. Please provide a valid status (Available, Requested, or Expired).");
            }
            var res = donorList.FindAll(x => x.Status.ToUpper() == status.ToUpper());
            res = res.FindAll(x => x.BloodType.ToUpper() == blood.ToUpper());
            if (res == null) { return NotFound(); };
            return res;
        }
    }
}
