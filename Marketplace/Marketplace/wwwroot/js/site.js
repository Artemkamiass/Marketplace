const profile = document.getElementById("profile");
const profileList = document.getElementById("profile-list");
const goods = document.getElementById("navbar")
const goodsList = document.getElementById("list-goods")
const exit = document.getElementById("exit")

profile.addEventListener("mouseenter", function () {
    profileList.style.display = "block";
});

profile.addEventListener("mouseleave", function () {
    profileList.style.display = "none";
});

profileList.addEventListener("mouseenter", function () {
    profileList.style.display = "block";
});

profileList.addEventListener("mouseleave", function () {
    profileList.style.display = "none";
});

goods.addEventListener("click", function () {
    goodsList.style.left = "0";
});

exit.addEventListener("click", function () {
    goodsList.style.left = "-510px";
});

