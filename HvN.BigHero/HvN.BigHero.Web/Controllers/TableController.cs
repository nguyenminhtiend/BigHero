using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using HvN.BigHero.DAL.Entities;
using HvN.BigHero.DAL.Service;
using HvN.BigHero.Web.Helper;

namespace HvN.BigHero.Web.Controllers
{
    public class TableController : BaseController
    {
        private readonly ITableService tableService;
        public TableController(ITableService tableService)
        {
            this.tableService = tableService;
        }
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(string tableName, int rowId)
        {
            var rowDetailViewModel = tableService.GetListColumnForEdit(tableName, rowId);
            return View(rowDetailViewModel);
        }
        public ActionResult Detail(string tableName)
        {
            var tableDetailViewModel = tableService.GeTableDetailViewModel(tableName);
            return View(tableDetailViewModel);
        }
        public ActionResult Add(string tableName)
        {
            var rowDetailViewModel = tableService.GetListColumnForAddNew(tableName);
            return View(rowDetailViewModel);
        }
        [HttpPost]
        public ActionResult DeleteRecord(string tableName, string primaryColumn, string rowId)
        {
            tableService.DeleteData(tableName, primaryColumn, rowId);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTableDetail(string tableName)
        {
            var tableDetailViewModel = tableService.GeTableDetailViewModel(tableName);
            return PartialView("_TableDetail", tableDetailViewModel);
        }

        [HttpPost]
        public ActionResult Save(string primaryKey, string tableName, string columns)
        {
            var primaryKeyValue = Request[primaryKey];
            var data = RequestCoverter.GetDataFromRequest(Request, columns);
            if (string.IsNullOrEmpty(primaryKeyValue))
            {
                tableService.InsertData(tableName, data);  
            }
            else
            {
                data.Add(primaryKey, primaryKeyValue);
                tableService.UpdateData(tableName, data);
            }
            return RedirectToAction("Detail", new {tableName });
        }

        public ActionResult GetMenuTable()
        {
            return PartialView("_MenuTable", tableService.GetTableMenu());
        }
        [HttpPost]
        public ActionResult CheckExistTable(string tableName)
        {
            var result = tableService.CheckExistTable(tableName);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult CreateTable(string tableName, string columns)
        {
            var lst = new JavaScriptSerializer().Deserialize<List<Column>>(columns);
            var saveTable = new Table { Name = tableName, IsIdentity = true, Columns = lst };
            tableService.AddNewTable(saveTable);
            return RedirectToAction("Detail", new { tableName });

        }
    }
}