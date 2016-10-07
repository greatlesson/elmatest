using Calc;
using Domain.Managers;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using WebCalc.Models;

namespace WebCalc.Controllers
{
    public class CalcController : Controller
    {
        private IHistoryManager Manager { get; set; }

        private IEnumerable<MethodInfo> Methods { get; set; }

        public CalcController()
        {
            Manager = new HistoryManager();
            Methods =
                typeof(Helper).GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
        }


        [HttpGet]
        public ActionResult Index()
        {
            return View(new CalcModel(Methods.Select(mi => mi.Name)));
        }

        [HttpPost]
        public ActionResult Index(CalcModel data)
        {
            var model = data ?? new CalcModel();

            var calcHelper = new Helper();

            if (ModelState.IsValid)
            {
                var oper = Methods.FirstOrDefault(m => m.Name == model.Operation);
                if (oper != null)
                {
                    var result = oper.Invoke(calcHelper, new object[] {model.X, model.Y});
                    model.Result = Convert.ToDouble(result);

                    AddOperation(model.X, model.Y, model.Result, oper.Name);

                    model.History = Manager.List(oper.Name);
                }
            }

            model.Operations = Methods.Select(o => new SelectListItem { Text = o.Name, Value = o.Name }).ToList();

            return View(model);
        }

        public ActionResult History()
        {
            return View(GetOperations());
        }

        #region РАБОТА С БД

        private void AddOperation(int x, int y, double? r, string oper)
        {
            var history = new History();

            history.CreationDate = DateTime.Now;
            history.X = x;
            history.Y = y;
            history.Result = r;
            history.Operation = oper;

            Manager.Add(history);
        }

        private IEnumerable<History> GetOperations()
        {
            return Manager.List();
        }


        #endregion

    }
}