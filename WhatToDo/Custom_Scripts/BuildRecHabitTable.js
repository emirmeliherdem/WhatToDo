$(document).ready(function () {
    $.ajax({
        url: '/RecommendedHabits/BuildRecHabitTable',
        success: function (result) {
            $('#recHabitTableDiv').html(result);
        }
    });
});