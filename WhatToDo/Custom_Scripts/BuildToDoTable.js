$(document).ready(function () {
    $.ajax({
        url: '/ToDoes/BuildToDoTable',
        success: function (result) {
            $('#toDoTableDiv').html(result);
        }
    });
});