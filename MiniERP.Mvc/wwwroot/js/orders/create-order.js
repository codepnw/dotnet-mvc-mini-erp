let index = 1;

const options = document.getElementById("product-options-template").innerHTML;

function updateSummary() {
  const rows = document.querySelectorAll("#items-table tbody tr");

  document.getElementById("item-count").textContent = rows.length;

  rows.forEach((row, i) => {
    row.querySelector(".row-number").textContent = i + 1;
  });
}

document.getElementById("add-item").addEventListener("click", () => {
  const tbody = document.querySelector("#items-table tbody");

  const row = `
            <tr>
                <td class="row-number">
                    ${index + 1}
                </td>

                <td>
                    <select name="Items[${index}].ProductId" class="form-select">
                        ${options}
                    </select>
                </td>

                <td>
                    <input
                        name="Items[${index}].Quantity"
                        type="number"
                        min="1"
                        value="1"
                        class="form-control"
                    />
                </td>

                <td>
                    <button type="button" class="btn btn-sm btn-outline-danger remove-row">
                        Remove
                    </button>
                </td>
            </tr>
        `;

  tbody.insertAdjacentHTML("beforeend", row);

  index++;

  updateSummary();
});

document.addEventListener("click", (e) => {
  if (e.target.classList.contains("remove-row")) {
    const rows = document.querySelectorAll("#items-table tbody tr");

    if (rows.length <= 1) {
      showToast("At least one item is required", "error");
      return;
    }

    e.target.closest("tr").remove();

    updateSummary();
  }
});

// -------------------- Modal Confirm Order --------------------

const modal = new bootstrap.Modal(
  document.getElementById("confirmCreateModal"),
);

document.getElementById("open-confirm-modal").addEventListener("click", () => {
  modal.show();
});

document.getElementById("confirm-submit").addEventListener("click", () => {
  document.getElementById("order-form").submit();
});
