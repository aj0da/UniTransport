// Mock data for demonstration
const mockData = {
    users: [
        { id: 1, name: 'John Doe', email: 'john@example.com', phone: '123-456-7890' },
        { id: 2, name: 'Jane Smith', email: 'jane@example.com', phone: '098-765-4321' }
    ],
    buses: [
        { id: 1, busNumber: 'BUS001', capacity: 50 },
        { id: 2, busNumber: 'BUS002', capacity: 45 }
    ],
    schedules: [
        { 
            id: 1, 
            busNumber: 'BUS001', 
            date: '2024-12-15', 
            time: '09:00', 
            seatsAvailable: 45, 
            totalSeats: 50,
            destination: 'Campus A',
            status: 'Active',
            price: '5 JD',
            departureLocation: 'Main Station'
        },
        { 
            id: 2, 
            busNumber: 'BUS002', 
            date: '2024-12-15', 
            time: '10:00', 
            seatsAvailable: 40, 
            totalSeats: 45,
            destination: 'Campus B',
            status: 'Active',
            price: '4 JD',
            departureLocation: 'North Station'
        }
    ],
    bookings: [
        { id: 1, studentName: 'Alice Johnson', tripDate: '2024-12-15', tripTime: '09:00', destination: 'Campus A', status: 'Pending' },
        { id: 2, studentName: 'Bob Wilson', tripDate: '2024-12-15', tripTime: '10:00', destination: 'Campus B', status: 'Approved' }
    ]
};

// Check authentication
//if (!localStorage.getItem("isLoggedIn") && !window.location.href.includes("login.html")) {
//    window.location.href = "login.html";
//}

document.addEventListener("DOMContentLoaded", () => {
    const sidebarLinks = document.querySelectorAll(".sidebar ul li a");
    const dynamicContent = document.getElementById("dynamic-content");
    const logoutBtn = document.getElementById("logout-btn");
    const searchInput = document.getElementById("searchInput");
    const modal = document.getElementById("modal");
    const modalContent = document.getElementById("modal-content");
    const closeModal = document.querySelector(".close");

    // Modal functions
    function showModal(content) {
        modalContent.innerHTML = content;
        modal.style.display = "block";
    }

    function hideModal() {
        modal.style.display = "none";
    }

    closeModal.onclick = hideModal;
    window.onclick = (event) => {
        if (event.target === modal) {
            hideModal();
        }
    }

    // Load content functions
    function loadUsers() {
        let content = `
            <div class="table-container">
                <h2>Manage Users</h2>
                <div class="action-buttons">
                    <input type="text" id="userSearch" placeholder="Search users...">
                </div>
                <table>
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Email</th>
                            <th>Phone Number</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        ${mockData.users.map(user => `
                            <tr>
                                <td>${user.name}</td>
                                <td>${user.email}</td>
                                <td>${user.phone}</td>
                                <td>
                                    <button class="action-button delete" onclick="deleteUser(${user.id})">Delete</button>
                                </td>
                            </tr>
                        `).join('')}
                    </tbody>
                </table>
            </div>
        `;
        dynamicContent.innerHTML = content;
    }

    function loadBuses() {
        let content = `
            <div class="table-container">
                <h2>Manage Buses</h2>
                <div class="action-buttons">
                    <button class="action-button" onclick="showAddBusModal()">Add New Bus</button>
                </div>
                <table>
                    <thead>
                        <tr>
                            <th>Bus Number</th>
                            <th>Capacity</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        ${mockData.buses.map(bus => `
                            <tr>
                                <td>${bus.busNumber}</td>
                                <td>${bus.capacity}</td>
                                <td>
                                    <button class="action-button delete" onclick="deleteBus(${bus.id})">Delete</button>
                                </td>
                            </tr>
                        `).join('')}
                    </tbody>
                </table>
            </div>
        `;
        dynamicContent.innerHTML = content;
    }

    function loadSchedules() {
        const today = new Date().toISOString().split('T')[0];
        let content = `
            <div class="table-container">
                <h2>Manage Schedules</h2>
                <div class="action-buttons">
                    <button class="action-button" onclick="showAddScheduleModal()">Add New Schedule</button>
                    <div class="filter-group">
                        <input type="date" id="scheduleFilterDate" min="${today}" 
                               onchange="filterSchedulesByDate(this.value)" placeholder="Filter by date">
                        <select id="statusFilter" onchange="filterSchedulesByStatus(this.value)">
                            <option value="all">All Status</option>
                            <option value="Active">Active</option>
                            <option value="Cancelled">Cancelled</option>
                            <option value="Completed">Completed</option>
                        </select>
                    </div>
                </div>
                <table>
                    <thead>
                        <tr>
                            <th>Bus Number</th>
                            <th>Date</th>
                            <th>Time</th>
                            <th>Departure</th>
                            <th>Destination</th>
                            <th>Available/Total Seats</th>
                            <th>Price</th>
                            <th>Status</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        ${mockData.schedules.map(schedule => `
                            <tr class="${schedule.seatsAvailable === 0 ? 'full-capacity' : ''}">
                                <td>${schedule.busNumber}</td>
                                <td>${schedule.date}</td>
                                <td>${schedule.time}</td>
                                <td>${schedule.departureLocation}</td>
                                <td>${schedule.destination}</td>
                                <td>${schedule.seatsAvailable}/${schedule.totalSeats}</td>
                                <td>${schedule.price}</td>
                                <td>
                                    <span class="status-badge ${schedule.status.toLowerCase()}">
                                        ${schedule.status}
                                    </span>
                                </td>
                                <td>
                                    <button class="action-button" onclick="editSchedule(${schedule.id})">
                                        <i class="fas fa-edit"></i> Edit
                                    </button>
                                    ${schedule.status === 'Active' ? `
                                        <button class="action-button warning" onclick="cancelSchedule(${schedule.id})">
                                            <i class="fas fa-ban"></i> Cancel
                                        </button>
                                    ` : ''}
                                    <button class="action-button delete" onclick="deleteSchedule(${schedule.id})">
                                        <i class="fas fa-trash"></i> Delete
                                    </button>
                                </td>
                            </tr>
                        `).join('')}
                    </tbody>
                </table>
            </div>
        `;
        dynamicContent.innerHTML = content;
    }

    function loadBookings() {
        let content = `
            <div class="table-container">
                <h2>Manage Bookings</h2>
                <table>
                    <thead>
                        <tr>
                            <th>Booking ID</th>
                            <th>Student Name</th>
                            <th>Trip Date</th>
                            <th>Trip Time</th>
                            <th>Destination</th>
                            <th>Status</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        ${mockData.bookings.map(booking => `
                            <tr>
                                <td>${booking.id}</td>
                                <td>${booking.studentName}</td>
                                <td>${booking.tripDate}</td>
                                <td>${booking.tripTime}</td>
                                <td>${booking.destination}</td>
                                <td>${booking.status}</td>
                                <td>
                                    ${booking.status === 'Pending' ? `
                                        <button class="action-button approve" onclick="approveBooking(${booking.id})">Approve</button>
                                        <button class="action-button reject" onclick="rejectBooking(${booking.id})">Reject</button>
                                    ` : ''}
                                </td>
                            </tr>
                        `).join('')}
                    </tbody>
                </table>
            </div>
        `;
        dynamicContent.innerHTML = content;
    }

    // Modal content generators
    window.showAddBusModal = function() {
        const modalHtml = `
            <h2>Add New Bus</h2>
            <form id="addBusForm">
                <div class="form-group">
                    <label for="busNumber">Bus Number</label>
                    <input type="text" id="busNumber" required>
                </div>
                <div class="form-group">
                    <label for="capacity">Capacity</label>
                    <input type="number" id="capacity" required>
                </div>
                <button type="submit" class="submit-btn">Add Bus</button>
            </form>
        `;
        showModal(modalHtml);

        document.getElementById('addBusForm').onsubmit = (e) => {
            e.preventDefault();
            if (confirm('Are you sure you want to add this bus?')) {
                // Add bus logic here
                alert('New bus added successfully!');
                hideModal();
                loadBuses();
            }
        };
    };

    window.showAddScheduleModal = function() {
        const modalHtml = `
            <h2>Add New Schedule</h2>
            <form id="addScheduleForm">
                <div class="form-group">
                    <label for="busNumber">Bus Number</label>
                    <select id="busNumber" required>
                        ${mockData.buses.map(bus => `
                            <option value="${bus.busNumber}">${bus.busNumber} (Capacity: ${bus.capacity})</option>
                        `).join('')}
                    </select>
                </div>
                <div class="form-group">
                    <label for="date">Date</label>
                    <input type="date" id="date" required min="${new Date().toISOString().split('T')[0]}">
                </div>
                <div class="form-group">
                    <label for="time">Time</label>
                    <input type="time" id="time" required>
                </div>
                <div class="form-group">
                    <label for="departureLocation">Departure Location</label>
                    <input type="text" id="departureLocation" required>
                </div>
                <div class="form-group">
                    <label for="destination">Destination</label>
                    <input type="text" id="destination" required>
                </div>
                <div class="form-group">
                    <label for="price">Price (JD)</label>
                    <input type="number" id="price" required min="0" step="0.5">
                </div>
                <button type="submit" class="submit-btn">Add Schedule</button>
            </form>
        `;
        showModal(modalHtml);

        document.getElementById('addScheduleForm').onsubmit = (e) => {
            e.preventDefault();

            // Validation checks
            const date = document.getElementById('date').value;
            const time = document.getElementById('time').value;
            const destination = document.getElementById('destination').value.trim();

            if (!date || !time || !destination) {
                alert('Please fill in all required fields.');
                return;
            }

            // Add schedule logic here
            alert('New schedule added successfully!');
            hideModal();
            loadSchedules();
        };
    };

    // Action functions
    window.deleteUser = function(userId) {
        if (confirm('Are you sure you want to delete this user?')) {
            // Delete user logic here
            loadUsers();
        }
    };

    window.deleteBus = function(busId) {
        if (confirm('Are you sure you want to delete this bus?')) {
            // Delete bus logic here
            loadBuses();
        }
    };

    window.editSchedule = function(scheduleId) {
        const schedule = mockData.schedules.find(s => s.id === scheduleId);
        const modalHtml = `
            <h2>Edit Schedule</h2>
            <form id="editScheduleForm">
                <div class="form-group">
                    <label for="busNumber">Bus Number</label>
                    <select id="busNumber" required>
                        ${mockData.buses.map(bus => `
                            <option value="${bus.busNumber}" ${bus.busNumber === schedule.busNumber ? 'selected' : ''}>
                                ${bus.busNumber}
                            </option>
                        `).join('')}
                    </select>
                </div>
                <div class="form-group">
                    <label for="date">Date</label>
                    <input type="date" id="date" value="${schedule.date}" required>
                </div>
                <div class="form-group">
                    <label for="time">Time</label>
                    <input type="time" id="time" value="${schedule.time}" required>
                </div>
                <div class="form-group">
                    <label for="destination">Destination</label>
                    <input type="text" id="destination" value="${schedule.destination}" required>
                </div>
                <button type="submit" class="submit-btn">Update Schedule</button>
            </form>
        `;
        showModal(modalHtml);

        document.getElementById('editScheduleForm').onsubmit = (e) => {
            e.preventDefault();
            // Update schedule logic here
            hideModal();
            loadSchedules();
        };
    };

    window.deleteSchedule = function(scheduleId) {
        if (confirm('Are you sure you want to delete this schedule?')) {
            // Delete schedule logic here
            loadSchedules();
        }
    };

    window.approveBooking = function(bookingId) {
        if (confirm('Are you sure you want to approve this booking?')) {
            // Approve booking logic here
            alert('Booking approved successfully!');
            loadBookings();
        }
    };

    window.rejectBooking = function(bookingId) {
        if (confirm('Are you sure you want to reject this booking?')) {
            // Reject booking logic here
            alert('Booking rejected successfully!');
            loadBookings();
        }
    };

    window.filterSchedulesByDate = function(date) {
        const filteredSchedules = date 
            ? mockData.schedules.filter(schedule => schedule.date === date)
            : mockData.schedules;
        updateScheduleTable(filteredSchedules);
    };

    window.filterSchedulesByStatus = function(status) {
        const filteredSchedules = status === 'all'
            ? mockData.schedules
            : mockData.schedules.filter(schedule => schedule.status === status);
        updateScheduleTable(filteredSchedules);
    };

    window.cancelSchedule = function(scheduleId) {
        if (confirm('Are you sure you want to cancel this schedule? This will notify all booked passengers.')) {
            // Cancel schedule logic here
            // Should include: 
            // 1. Update schedule status
            // 2. Notify booked passengers
            // 3. Handle refunds if necessary
            loadSchedules();
        }
    };

    function updateScheduleTable(schedules) {
        const tbody = document.querySelector('tbody');
        if (tbody) {
            tbody.innerHTML = schedules.map(schedule => `
                <tr class="${schedule.seatsAvailable === 0 ? 'full-capacity' : ''}">
                    <td>${schedule.busNumber}</td>
                    <td>${schedule.date}</td>
                    <td>${schedule.time}</td>
                    <td>${schedule.departureLocation}</td>
                    <td>${schedule.destination}</td>
                    <td>${schedule.seatsAvailable}/${schedule.totalSeats}</td>
                    <td>${schedule.price}</td>
                    <td>
                        <span class="status-badge ${schedule.status.toLowerCase()}">
                            ${schedule.status}
                        </span>
                    </td>
                    <td>
                        <button class="action-button" onclick="editSchedule(${schedule.id})">
                            <i class="fas fa-edit"></i> Edit
                        </button>
                        ${schedule.status === 'Active' ? `
                            <button class="action-button warning" onclick="cancelSchedule(${schedule.id})">
                                <i class="fas fa-ban"></i> Cancel
                            </button>
                        ` : ''}
                        <button class="action-button delete" onclick="deleteSchedule(${schedule.id})">
                            <i class="fas fa-trash"></i> Delete
                        </button>
                    </td>
                </tr>
            `).join('');
        }
    }

    // Search functionality
    searchInput.addEventListener("input", (e) => {
        const searchTerm = e.target.value.toLowerCase();
        const currentSection = document.querySelector('.sidebar a.active').getAttribute('href').substring(1);
        
        switch(currentSection) {
            case 'users':
                searchUsers(searchTerm);
                break;
            case 'buses':
                searchBuses(searchTerm);
                break;
            case 'schedules':
                searchSchedules(searchTerm);
                break;
            case 'bookings':
                searchBookings(searchTerm);
                break;
        }
    });

    // Search functions for each section
    function searchUsers(term) {
        const filteredUsers = mockData.users.filter(user => 
            user.name.toLowerCase().includes(term) ||
            user.email.toLowerCase().includes(term) ||
            user.phone.toLowerCase().includes(term)
        );

        const tbody = document.querySelector('tbody');
        if (tbody) {
            tbody.innerHTML = filteredUsers.map(user => `
                <tr>
                    <td>${user.name}</td>
                    <td>${user.email}</td>
                    <td>${user.phone}</td>
                    <td>
                        <button class="action-button delete" onclick="deleteUser(${user.id})">Delete</button>
                    </td>
                </tr>
            `).join('');
        }
    }

    function searchBuses(term) {
        const filteredBuses = mockData.buses.filter(bus => 
            bus.busNumber.toLowerCase().includes(term) ||
            bus.capacity.toString().includes(term)
        );

        const tbody = document.querySelector('tbody');
        if (tbody) {
            tbody.innerHTML = filteredBuses.map(bus => `
                <tr>
                    <td>${bus.busNumber}</td>
                    <td>${bus.capacity}</td>
                    <td>
                        <button class="action-button delete" onclick="deleteBus(${bus.id})">Delete</button>
                    </td>
                </tr>
            `).join('');
        }
    }

    function searchSchedules(term) {
        const filteredSchedules = mockData.schedules.filter(schedule => 
            schedule.busNumber.toLowerCase().includes(term) ||
            schedule.date.includes(term) ||
            schedule.time.includes(term) ||
            schedule.destination.toLowerCase().includes(term) ||
            schedule.seatsAvailable.toString().includes(term)
        );

        const tbody = document.querySelector('tbody');
        if (tbody) {
            tbody.innerHTML = filteredSchedules.map(schedule => `
                <tr>
                    <td>${schedule.busNumber}</td>
                    <td>${schedule.date}</td>
                    <td>${schedule.time}</td>
                    <td>${schedule.seatsAvailable}</td>
                    <td>${schedule.destination}</td>
                    <td>
                        <button class="action-button" onclick="editSchedule(${schedule.id})">Edit</button>
                        <button class="action-button delete" onclick="deleteSchedule(${schedule.id})">Delete</button>
                    </td>
                </tr>
            `).join('');
        }
    }

    function searchBookings(term) {
        const filteredBookings = mockData.bookings.filter(booking => 
            booking.id.toString().includes(term) ||
            booking.studentName.toLowerCase().includes(term) ||
            booking.tripDate.includes(term) ||
            booking.tripTime.includes(term) ||
            booking.destination.toLowerCase().includes(term) ||
            booking.status.toLowerCase().includes(term)
        );

        const tbody = document.querySelector('tbody');
        if (tbody) {
            tbody.innerHTML = filteredBookings.map(booking => `
                <tr>
                    <td>${booking.id}</td>
                    <td>${booking.studentName}</td>
                    <td>${booking.tripDate}</td>
                    <td>${booking.tripTime}</td>
                    <td>${booking.destination}</td>
                    <td>${booking.status}</td>
                    <td>
                        ${booking.status === 'Pending' ? `
                            <button class="action-button approve" onclick="approveBooking(${booking.id})">Approve</button>
                            <button class="action-button reject" onclick="rejectBooking(${booking.id})">Reject</button>
                        ` : ''}
                    </td>
                </tr>
            `).join('');
        }
    }

    // Event listeners
    sidebarLinks.forEach(link => {
        link.addEventListener("click", (e) => {
            e.preventDefault();
            const section = link.getAttribute("href").substring(1);
            
            // Update active link
            sidebarLinks.forEach(l => l.classList.remove("active"));
            link.classList.add("active");

            // Load appropriate content
            switch(section) {
                case 'users':
                    loadUsers();
                    break;
                case 'buses':
                    loadBuses();
                    break;
                case 'schedules':
                    loadSchedules();
                    break;
                case 'bookings':
                    loadBookings();
                    break;
            }
        });
    });

    // Logout functionality
    logoutBtn.addEventListener("click", () => {
        localStorage.removeItem("isLoggedIn");
        window.location.href = "login.html";
    });

    // Load default content (users)
    loadUsers();
});

// Login Logic
if (window.location.href.includes("login.html")) {
    document.getElementById("loginForm")?.addEventListener("submit", (event) => {
        event.preventDefault();

        const username = document.getElementById("username").value;
        const email = document.getElementById("email").value;

        // Simple validation for demonstration
        if (username === "admin" && email === "admin@example.com") {
            alert("Login successful!");
            localStorage.setItem("isLoggedIn", "true"); // Set login status
            window.location.href = "dashboard.html"; // Redirect to dashboard
        } else {
            alert("Invalid username or email!");
        }
    });
}
