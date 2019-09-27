'use strict';

app.controller('demoController', function ($scope, $location, taskMainServices) {

    $scope.taskDescription = '';
    $scope.taskDate = '';
    $scope.assignee = '';
    $scope.tasks = [];
    $scope.statuses = listStatuses;

    $scope.developers = listOfDevelopers;

    $scope.getListOfTasks = function () {

        $.blockUI();

        taskMainServices.getAllTasks()
        .then(
              function (result) {
                  $scope.tasks = result.data;
                  $scope.DisplayStatistics();
                  $.unblockUI();
              },
              function (error) {
                  alert('error');

                  $.unblockUI();

              });
    };

    $scope.Initialize = function () {
        $scope.getListOfTasks();
    };

    $scope.AddTask = function () {
        $scope.taskObject = { description: $scope.taskDescription, assignee: $scope.assignee, date: $scope.taskDate, status: active };

        taskMainServices.addNewTask($scope.taskObject)
        .then(
              function (result) {

                  $scope.Initialize();
                  $scope.DisplayStatistics();
                  $.unblockUI();
              },
              function (error) {
                  alert('error');
                  $.unblockUI();

              });
        //$scope.tasks.push($scope.taskObject);
    };

    $scope.deleteTask = function (id) {
        taskMainServices.deleteTask(id)
        .then(
              function (result) {

                  $scope.Initialize();
                  $scope.DisplayStatistics();
                  $.unblockUI();
              },
              function (error) {
                  alert('error');
                  $.unblockUI();
              });
    };

    $scope.UpdateTask = function () {

        taskMainServices.updateTask($scope.selectedTask)
        .then(
              function (result) {

                  $scope.Initialize();
                  $scope.DisplayStatistics();
                  $("#updateTask").modal('hide');
                  $.unblockUI();
              },
              function (error) {
                  alert('error');
                  $("#updateTask").modal('hide');
                  $.unblockUI();

              });
        //$scope.tasks.push($scope.taskObject);
    };

    $scope.setCurrentTask = function (task) {
        $scope.selectedTask = task;
    };

    $scope.DisplayStatistics = function () {
        displayChartByStatus();
        displayChartByAssignee();
    };

    $scope.returnStatusStats = function (tasks) {
        var activeItems = [];
        var doneItems = [];
        var inProgressItems = [];

        tasks.forEach
            (
               function (item) {
                   if (item.Status == 'Active')
                       activeItems.push(item);
                   else if (item.Status == 'Done')
                       doneItems.push(item);
                   else if (item.Status == 'In Progress')
                       inProgressItems.push(item);
               }
            );


        var obj1 = { "label": "Active", "value": activeItems.length }
        var obj2 = { "label": "In Progress", "value": inProgressItems.length }
        var obj3 = { "label": "Done", "value": doneItems.length }

        var result = [obj1, obj2, obj3];
        return result;
    };

    function displayChartByStatus() {
        $scope.data = [];
        var result = $scope.returnStatusStats($scope.tasks);

        $scope.data.push(result[0]);
        $scope.data.push(result[1]);
        $scope.data.push(result[2]);

        var revenueChart = new FusionCharts({
            type: "column3d",
            renderAt: "statusChartContainer",
            width: "500",
            height: "300",
            dataFormat: "json",
            dataSource: {
                "chart": {
                    "caption": "",
                    "subCaption": "",
                    "xAxisName": "Status",
                    "yAxisName": "Number",
                    "theme": "zune"
                },
                "data": $scope.data
            }
        });
        revenueChart.render("statusChartContainer");
    }

    $scope.returnAssigneeStats = function (tasks) {
        var ahmedItems = [];
        var crisItems = [];
        var manuItems = [];


        tasks.forEach
        (
            function (item) {
                if (item.Assignee == 'Theljani Ahmed')
                    ahmedItems.push(item);
                else if (item.Assignee == 'Cristophe Garcia')
                    crisItems.push(item);
                else if (item.Assignee == 'Manu Thibaud')
                    manuItems.push(item);
            }
        );

        var obj1 = { "label": "Theljani Ahmed", "value": ahmedItems.length }
        var obj2 = { "label": "Cristophe Garcia", "value": crisItems.length }
        var obj3 = { "label": "Manu Thibaud", "value": manuItems.length }

        var result = [obj1, obj2, obj3];
        return result;
    };

    function displayChartByAssignee() {
        $scope.data = [];
        var result = $scope.returnAssigneeStats($scope.tasks);

        $scope.data.push(result[0]);
        $scope.data.push(result[1]);
        $scope.data.push(result[2]);
        $scope.data.push(result[3]);
        $scope.data.push(result[4]);

        var revenueChart = new FusionCharts({
            type: "column3d",
            renderAt: "assigneeChartContainer",
            width: "500",
            height: "300",
            dataFormat: "json",
            dataSource: {
                "chart": {
                    "caption": "",
                    "subCaption": "",
                    "xAxisName": "Assignee",
                    "yAxisName": "Taks",
                    "theme": "zune"
                },
                "data": $scope.data
            }
        });

        revenueChart.render("assigneeChartContainer");
    }

    $scope.Initialize();
});