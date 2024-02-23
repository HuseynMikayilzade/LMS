//======== Custom delete button ======//
const deletebtns = document.querySelectorAll(".customDeletebtn")
deletebtns.forEach(btn => {
    btn.addEventListener("click", function (e) {
        e.preventDefault();
        var endpoint = btn.getAttribute("href");
        const swalWithBootstrapButtons = Swal.mixin({
            customClass: {
                confirmButton: "btn btn-success",
                cancelButton: "btn btn-danger"
            },
            buttonsStyling: false
        });
        swalWithBootstrapButtons.fire({
            title: "Are you sure?",
            text: "You won't be able to revert this!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Yes, delete it!",
            cancelButtonText: "No, cancel!",
            reverseButtons: true
        }).then((result) => {
            if (result.isConfirmed) {
                fetch(endpoint)
                    .then(response => response.json())
                    .then(data => {
                        if (data.status == 200) {
                            window.location.reload(true);
                        } else {
                            swalWithBootstrapButtons.fire({
                                title: "Error!",
                                text: "Your file hasn't been deleted.",
                                icon: "error"
                            });
                        }
                    })
            } else if (

                result.dismiss === Swal.DismissReason.cancel
            ) {
                swalWithBootstrapButtons.fire({
                    title: "Cancelled",
                    text: "Your imaginary file is safe :)",
                    icon: "error"
                });
            }
        });
    })
})










//======== Create Task =======//
const btn = document.getElementById("CreateTask");
btn.addEventListener("click", function (b) {
    b.preventDefault();
    var form = document.getElementById("createTaskForm");
    var groupid = window.location.href.split("/")[5];
    const data = new FormData();
    var isaviable = document.getElementById("IsAvailable").checked;
    var taskname = document.getElementById("TaskName").value;
    var maxpoint = document.getElementById("MaxPoint").value;
    var tasktype = document.getElementById("TaskType").value;
    var starttime = document.getElementById("StartTime").value;
    var endtime = document.getElementById("EndTime").value;
    var desc = document.getElementById("Description").value;
    data.append("IsActive", isaviable);
    data.append("Name", taskname);
    data.append("MaxPoint", maxpoint);
    data.append("TaskType", tasktype);
    data.append("StartTime", starttime);
    data.append("EndTime", endtime);
    data.append("Description", desc);
    data.append("GroupId", groupid);

    const url = "https://localhost:7064/manage/assignment/create";
    fetch(url, {
        method: "POST",
        body: data
    })
        .then((res) => res.json())
        .then(function (data) {

            if (data.statusCode === 200) {
                alert(data.message);
                var modal = document.getElementById("createTaskModal");
                modal.style.display = "none";
                form.reset();
                var span = document.getElementById("errorSpan");
                span.innerHTML = "";
                var close = document.getElementById("closeBtn");
                close.click();
            }
            else {
                var span = document.getElementById("errorSpan");
                span.innerHTML = data.message.replace("\n", '<br />');
            }
        });
});

var closeBtn = document.getElementById("closeBtn");
closeBtn.addEventListener("click", function (b) {
    b.preventDefault();
    var form = document.getElementById("createTaskForm");
    form.reset();
    var span = document.getElementById("errorSpan");
    span.innerHTML = "";
});





//========== Active/Deactive task =========//
function activateTask(id) {
    fetch('/manage/Assignment/ActivateTask/' + id, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            location.reload();
        })
        .catch(error => {
            console.error('There was a problem with your fetch operation:', error);
        });
}
function deactivateTask(id) {
    fetch('/manage/Assignment/DeActivateTask/' + id, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            location.reload();
        })
        .catch(error => {
            console.error('There was a problem with your fetch operation:', error);
        });
}




//=============== Toggle Button ==============//
document.addEventListener("DOMContentLoaded", function () {
    var toggleBtn = document.getElementById("toggle_btn");
    var body = document.body;

    toggleBtn.addEventListener("click", function () {
        body.classList.toggle("mini-sidebar");
    });
});

document.addEventListener("DOMContentLoaded", function () {
    var body = document.body;
    var sidebar = document.getElementById("sidebar");

    var isMouseOverSidebar = false;

    sidebar.addEventListener("mouseover", function () {
        isMouseOverSidebar = true;
        body.classList.add("expand-menu");
    });

    sidebar.addEventListener("mouseout", function () {
        isMouseOverSidebar = false;
        if (!isMouseOverSidebar) {
            body.classList.remove("expand-menu");
        }
    });

    body.addEventListener("mouseover", function () {
        if (!isMouseOverSidebar) {
            body.classList.remove("expand-menu");
        }
    });
});
