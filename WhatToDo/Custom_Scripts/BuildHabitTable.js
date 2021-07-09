$(document).ready(function () {
    $.ajax({
        url: '/Habits/BuildHabitTable',
        success: function (result) {
            $('#habitTableDiv').html(result);
        }
    });
});