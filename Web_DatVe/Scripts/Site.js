/******************************
 * DARK MODE SYSTEM
 ******************************/

const body = document.body;
const icon = document.getElementById("darkIcon");
const logo = document.getElementById("logoMain");

// Áp dụng Dark Mode
function applyDarkMode(isDark) {
    if (isDark) {
        body.classList.add("dark");
        icon.classList.remove("bi-moon-stars");
        icon.classList.add("bi-brightness-high");
        // Chỉ thay đổi filter, không thay đổi đường dẫn logo
        if (logo) {
            // Đảm bảo logo dùng đúng đường dẫn
            const currentSrc = logo.getAttribute("src") || logo.src;
            if (!currentSrc.includes("Content/images/logo-light.jpg") && 
                !currentSrc.includes("Content/images/logo-dark.jpg")) {
                logo.src = "/Content/images/logo-light.jpg";
            }
            logo.style.filter = "brightness(1.2) contrast(1.1)"; // Làm sáng logo trong dark mode
        }
    } else {
        body.classList.remove("dark");
        icon.classList.remove("bi-brightness-high");
        icon.classList.add("bi-moon-stars");
        if (logo) {
            // Đảm bảo logo dùng đúng đường dẫn
            const currentSrc = logo.getAttribute("src") || logo.src;
            if (!currentSrc.includes("Content/images/logo-light.jpg") && 
                !currentSrc.includes("Content/images/logo-dark.jpg")) {
                logo.src = "/Content/images/logo-light.jpg";
            }
            logo.style.filter = "none"; // Bỏ filter trong light mode
        }
    }
}

// Đợi DOM load xong trước khi áp dụng dark mode
document.addEventListener("DOMContentLoaded", function() {
    // Đọc trạng thái từ localStorage
    const savedMode = localStorage.getItem("darkmode");
    applyDarkMode(savedMode === "true");
});

// Toggle Dark Mode
document.getElementById("darkModeToggle")?.addEventListener("click", () => {
    const isDark = !body.classList.contains("dark");
    applyDarkMode(isDark);
    localStorage.setItem("darkmode", isDark);
});


/******************************
 * SEAT MAP (GHẾ CGV 3D STYLE)
 ******************************/

// DANH SÁCH GHẾ NGƯỜI DÙNG ĐÃ CHỌN
let selectedSeats = [];

// Khi người dùng nhấn vào 1 ghế
document.addEventListener("click", function (e) {
    if (!e.target.classList.contains("seat")) return;

    let seat = e.target;

    // Nếu ghế đã đặt → không chọn được
    if (seat.classList.contains("disabled")) return;

    let seatId = seat.getAttribute("data-id");

    if (seat.classList.contains("selected")) {
        // BỎ CHỌN GHẾ
        seat.classList.remove("selected");
        selectedSeats = selectedSeats.filter(x => x !== seatId);
    } else {
        // CHỌN GHẾ
        seat.classList.add("selected");
        if (!selectedSeats.includes(seatId)) selectedSeats.push(seatId);
    }

    updateSeatSummary();
});

// Cập nhật danh sách ghế được chọn ở UI
function updateSeatSummary() {
    const panel = document.getElementById("selectedSeatList");
    if (!panel) return;

    panel.innerHTML = selectedSeats.length
        ? selectedSeats.join(", ")
        : "Chưa chọn ghế nào";
}


/******************************
 * GỬI GHẾ ĐÃ CHỌN VỀ FORM
 ******************************/

// Form submit để đặt ghế
document.getElementById("seatSubmitBtn")?.addEventListener("click", () => {
    if (selectedSeats.length === 0) {
        alert("Vui lòng chọn ghế trước!");
        return false;
    }

    let hiddenInput = document.getElementById("SeatIds");
    if (hiddenInput) {
        hiddenInput.value = selectedSeats.join(",");
    }
});


/******************************
 * UI EFFECTS – CUỘN MỀM, HOVER
 ******************************/

// Cuộn mềm (smooth scroll)
document.querySelectorAll("a[href^='#']").forEach(anchor => {
    anchor.addEventListener("click", function (e) {
        e.preventDefault();
        let target = document.querySelector(this.getAttribute("href"));
        if (target) {
            target.scrollIntoView({ behavior: "smooth" });
        }
    });
});


/******************************
 * LOG
 ******************************/
console.log("Nhom8Cinema UI loaded successfully.");
