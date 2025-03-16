$(document).ready(function () {
    $("#searchBtn").click(function () {
        var name = $("#searchInput").val();
        $.get("/Employee/Search?name=" + name, function (data) {
            $("#employeeTableBody").html(data);
        });
    });
});
