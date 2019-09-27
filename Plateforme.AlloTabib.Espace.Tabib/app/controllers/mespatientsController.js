'use strict';

app.controller('mespatientsController', function ($scope, $location, $window, calendrierApiServices) {

    //20-01-2015
    $.unblockUI();
    var userName = $window.sessionStorage["userName"];

    if (userName == '' || userName == null || userName == undefined)
        $location.path('/');

    // Pagination
    $scope.totalRecordsCount = 0;
    $scope.totalPages = 0;
    $scope.pageSize = 5;
    $scope.currentPage = 1;
    $scope.maxSize = 5;


    getPatientParPraticien(userName);

    $scope.patients = [];
    function getPatientParPraticien(userName)
    {
        calendrierApiServices.patients(userName, ($scope.currentPage - 1) * $scope.pageSize, $scope.pageSize).then(
                // Case : Success.
                         function (result) {

                             $.unblockUI();
                             $scope.patients = result.data.data.items;
                                                    
                             if ($scope.patients.length > 0) {

                                 // Get the pagination parameters (total items count and pages count)
                                 var paginationHeader = result.data.data.paginationHeader;

                                 // Set the board name and the selected board.
                                 $scope.selectedPatient = result.data.data.items[0];
                                 $scope.nomPrenom = $scope.selectedPatient.nomPrenom;

                                 if ($scope.patients.count != 0) {
                                     $scope.$on('ngRepeatFinished', function (ngRepeatFinishedEvent) {
                                         $scope.getPatientDetails($scope.selectedPatient);
                                    
                                     });
                                 }
                             }
                             else {
                                 // If the list of board is empty, this message will be shown.
                                 $scope.transactionState = 'Aucun patient est présent actuellement.';
                             }
                             // Stop the progress bar.
                             $.unblockUI();
                         },
                         // Case : Error.
                         function (error) {
                             $scope.errorMessageLoginTxt = "Problème de récupération des patients, veuillez nous contacter. Equipe AlloTabib."
                         }
                );
        $.unblockUI();
    }

    $scope.Initialize = function () {
        $scope.searchPatient = '';
    };

    
    $scope.getPatientDetails = function (patient) {

        if (!$scope.oldSelectedPatient) {
            $scope.oldSelectedPatient = patient;
        }

        if ($scope.oldSelectedPatient != patient) {
            var e = document.getElementById("patientPanel_" + $scope.oldSelectedPatient.cin);
            if (e) {

                var index = e.className.indexOf(" bubble");
                if (index > -1) {
                    e.className = e.className.substring(0, index);
                }
            }
            $scope.oldSelectedPatient = patient;
        }

        var e = document.getElementById("patientPanel_" + $scope.oldSelectedPatient.cin);

        setTimeout(function () {
            if (e)
                e.className += " bubble text-primary";
        }, 10);


        $scope.selectedPatient = patient;
        $scope.nomPrenom = patient.nomPrenom;
    };

});