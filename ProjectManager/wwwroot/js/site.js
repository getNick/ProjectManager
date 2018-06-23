// Write your JavaScript code.
function GetRecomendation() {
    var data = {};
    data.TeamSize = document.getElementById("teamSize").value;
    data.Innovation = document.getElementById("innovation").checked;
    data.CrearCutIdea = document.getElementById("clearCutIdea").checked;
    data.RegularCommunication = document.getElementById("regularCommunication").checked;
    data.CustomerPartner = document.getElementById("customerPartner").checked;

    $.ajax({
        type: "POST",
        url: "/Project/Recomendation",
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        data: JSON.stringify(data),
        error: function (response) {
            console.log(response);
        },
        success: function (result) {
            console.log(result);

            $('#page').html("<p> The most suitable methodology for you is Scrum " +result.Scrum + "  %</p>");

        }
    });
}
        var counter = 1;

        $("#addExpression").on("click", function () {

            var template= $("#template").html();
            var res = template.replace("variable[0]", "variable[" + counter + "]");
            var res = res.replace("sign[0]", "sign[" + counter + "]");
            var res = res.replace("value[0]", "value[" + counter + "]");
            $("#expressionTable").append(res);
            counter++;
        });



        $("#expressionTable").on("click", ".ibtnDel", function (event) {
            $(this).closest("tr").remove();
            counter -= 1;
        });