const modal = document.querySelector(".modal");
const btn = document.querySelector(".js-icon-plus");
const btnClose = document.querySelectorAll(".js-close-modal");
function showAddClient() {
    modal.classList.add('open');
}        
function closeAddClient() {
    modal.classList.remove('open');
}
btn.addEventListener('click', showAddClient);
for(const close of btnClose) {
    close.addEventListener('click', closeAddClient);
}
