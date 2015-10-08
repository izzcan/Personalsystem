$(document).ready(function () {
    // Populate categories when the page is loaded.
    $.getJSON('/Interviews/GetVacancies', function (data) {
        // Ajax success callback function. Populate dropdown from Json data returned from server.
        $('#vacancies option').remove();
        $('#vacancies').append('<option value="">--Välj tjänst--</option');
        for (i = 0; i < data.length; i++) {
            $('#vacancies').append('<option value="' + data[i].Id + '">' + data[i].Title + '</option');
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        // Ajax fail callback function.
        alert('Kunde inte hitta några tjänster!');
    });

    // First dropdown selection change event handler
    $('#vacancies').change(function () {
        var vacId = $(this).find(":selected").val();
        if (vacId.length > 0) {
            // Populate products.
            $.getJSON('/Interviews/GetApplicants', { vacId: vacId }, function (data) {
                // Ajax success callback function. Populate dropdown from Json data returned from server.
                $('#Application_Id option').remove();
                $('#Application_Id').append('<option value="">--Välj sökande--</option');
                for (i = 0; i < data.length; i++) {
                    $('#Application_Id').append('<option value="' + data[i].Id + '">' + data[i].Email + '</option');
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                // Ajax fail callback function.
                debugger;
                alert('Kunde inte hitta några sökande!');
            });
        }
        else {
            // Remove all second dropdown options if empty option is selected in first dropdown.
            $('#Application_Id option').remove();
            $('#Application_Id').append('<option value=""></option');
        }
    });
});
