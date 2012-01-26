

var automagic = (function () {
    ///<summary>The köstum module for letting web admin adding fancy features and have them working right now, there and then, automagically!</summary>
    //*** PRIVATE

    //*** PUBLIC
    return {
    }
})();

automagic.selectUrl = (function (app, global) {
    ///<summary>Search dom onload for dropdowns with a specified id, redirect browser to selected value onChange.</summary>
    //*** PRIVATE

    var amId = "amSelectUrl";

    function redirect(dropdownNode) {
        var url = $(dropdownNode).val();
        top.location.href = url;
    }

    //Set events
    $("#" + amId).live("change", function () {
        var node = this;
        redirect(node);
    });

    function Testing(result, input, lol2) {
        var test = "testing";
    }

    //*** PUBLIC
    return {
        MyFunction: Testing,
        MyFunction2: function (result, input, lol2) {
            var testing2 = "testing2";
        }
    };

})(automagic, this);


noiseCalculator.selectUrl.js
