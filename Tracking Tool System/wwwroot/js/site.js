
document.addEventListener("DOMContentLoaded", function () {

    document.querySelectorAll(".table-scroll-container").forEach(function (container) {

        const topScroll = container.querySelector(".horizontal-scroll");
        const tableScroll = container.querySelector(".table-scroll");
        const table = tableScroll.querySelector("table");
        const topContent = container.querySelector(".horizontal-scroll-content");

        if (!topScroll || !tableScroll || !table || !topContent) {
            return;
        }

        topContent.style.width = table.offsetWidth + "px";

        topScroll.addEventListener("scroll", function () {
            tableScroll.scrollLeft = topScroll.scrollLeft;
        });

        tableScroll.addEventListener("scroll", function () {
            topScroll.scrollLeft = tableScroll.scrollLeft;
        });

    });

});