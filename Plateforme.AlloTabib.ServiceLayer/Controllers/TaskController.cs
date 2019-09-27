using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.TaskAppServices;
using Plateforme.AlloTabib.DomainLayer.Entities;

namespace Plateforme.AlloTabib.ServiceLayer.Controllers
{
    [RoutePrefix("tasks")]
    public class TaskController : ApiController
    {
        private readonly ITaskApplicationServices _taskApiApplicationServices;

        public TaskController(ITaskApplicationServices taskApiApplicationServices)
        {
            _taskApiApplicationServices = taskApiApplicationServices;
        }


        [Route("")]
        public IEnumerable<Task> GetAll()
        {
            return _taskApiApplicationServices.GetAll();
        }

        [Route("new")]
        public void Post(Task task)
        {
            if (task != null)
                _taskApiApplicationServices.AddNewTask(task);
        }

        [Route("modify")]
        public void Put(Task task)
        {
            if (task != null)
                _taskApiApplicationServices.ModifyTask(task);
        }

        [Route("delete/{id}")]
        public void Delete(int id)
        {
            _taskApiApplicationServices.DeleteTask(id);
        }
    }
}
