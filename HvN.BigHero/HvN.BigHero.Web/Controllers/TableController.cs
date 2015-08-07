using System.Web.Mvc;
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
            return PartialView("TableDetail", tableDetailViewModel);
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
    }
}