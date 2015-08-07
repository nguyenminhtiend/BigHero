﻿using System.Web.Mvc;
using HvN.BigHero.DAL.Service;
using HvN.BigHero.Web.Helper;

namespace HvN.BigHero.Web.Controllers
{
    public class TableController : Controller
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
            return View();
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
        public ActionResult Add()
        {
            var tableName = Request["TableName"];
            tableService.InsertData(tableName, RequestCoverter.GetDataFromRequest(Request, Request["Columns"]));
            return RedirectToAction("Detail", new {tableName });
        }
    }
}