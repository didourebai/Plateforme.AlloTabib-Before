'use strict';
//Add factory 
app.factory('patientApiServices', function ($http, appSettings) {

    var patientServiceBase = appSettings.patientUri;
    //declare factory object for this service
    var patientApiServiceDataFactory = {};
    /****************************************** Private Methods. ******************************************
   /*****************************************************************************************************/
    // Add new patient.
    var _addPatient = function (patient) {

        var patientServiceAddUrl = patientServiceBase + 'new';
        return $http({
            method: 'POST',
            url: patientServiceAddUrl,
            data: patient
        }).then(function (results) {
            return results;
        });
    };



    /****************************************** Public Methods. ******************************************
   /*****************************************************************************************************/

    patientApiServiceDataFactory.addPatient = _addPatient;
    // Return.
    return patientApiServiceDataFactory;
});