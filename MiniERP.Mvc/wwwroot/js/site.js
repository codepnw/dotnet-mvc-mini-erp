// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function showToast(message, type = "success") {
  const toastEl = document.getElementById("app-toast");

  const header = document.getElementById("toast-header");

  const title = document.getElementById("toast-title");

  toastEl.querySelector(".toast-body").textContent = message;

  header.className = "toast-header";

  if (type === "success") {
    header.classList.add("bg-success", "text-white");
    title.textContent = "Success";
  }

  if (type === "error") {
    header.classList.add("bg-danger", "text-white");
    title.textContent = "Error";
  }

  const toast = new bootstrap.Toast(toastEl);

  toast.show();
}
