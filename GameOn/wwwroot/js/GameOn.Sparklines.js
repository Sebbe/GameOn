function RenderSparklines() {
    // Summary: Renders all sparklines on a page. Makes an AJAX call to get the Sparklines data, then attempts to render a sparkline for any tag 
    //          with class "sparkline" and a "data-playerid" attribute.
    // Remarks: Use of AJAX GET is deliberate. Sparkline data is ideal for caching (by the browser) and is a read-only operation.
    $.ajax("/Players/GetSparklinesData", {
        type: "GET",
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                $(".sparkline[data-playerid = " + data[i].item1 + "]").sparkline(data[i].item2.split(","));
            }
        }
    });   
}