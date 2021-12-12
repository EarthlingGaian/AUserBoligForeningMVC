//functional style programming

//'let' can only be defined ones in each scope (scoped block)

let clicked = null; //whatever day we click on
let events = []; //array of event objects. U can only store strings in local storage

const calendar = document.getElementById('calendar');
const newEventModal = document.getElementById('newEventModal');
const deleteEventModal = document.getElementById('deleteEventModal');
const backDrop = document.getElementById('modalBackDrop');
const eventTitleInput = document.getElementById('eventTitleInput');
window.localStorage.clear();


function openModal(date) { //event for save and cancel button
    window.localStorage.clear();
   
    clicked = date; //clecked on a date

    const eventForDay = events.find(e => e.date === clicked);//check if there is a event for the given day

    if (eventForDay) {
        document.getElementById('eventText').innerText = eventForDay.title; //set local since it is the only time it wil be referenced
        deleteEventModal.style.display = 'block';

    } else {
        newEventModal.style.display = 'block'; //block gets a pop-vindoe
    }

    backDrop.style.display = 'block';
}

function load() {
    
    window.localStorage.clear();
    
    //get data from db -- data loaded in sourcearray which is defined in CalendarInex under scripts
    var arrayLength = sourcearray.length;
    for (var i = 0; i < arrayLength; i++) {

        //console.log(sourcearray[i]['date']);
        //console.log(sourcearray[i]['title']);

        events.push({
            date: sourcearray[i]['date'],
            title: sourcearray[i]['title']
        });
        localStorage.setItem('events', JSON.stringify(events));  //we have to use JSON.stringify() since we cant store a object in local storage
    }





    const dt = new Date(); //creating a date object -- Gets the current date of when used



    const day = dt.getDate(); //current day
    const month = dt.getMonth(); //current month remember starts at 0 indexing
    const year = dt.getFullYear(); //current year


    //get the first day of a month
    //const firstDayOfMonth = new Date(year, month, 1); //gets the current first of month

    //Get all days in a given month
    const daysInMonth = new Date(year, month + 1, 0).getDate(); // 0 gives the last day of previus month and month +1 gives the next month
    // so if we want all days in januar we ask for februar ( month + 1) og går en dag tilbage (0) og ermed for en sidste dag af januar 
    //.getDate() converts it to day number



    //console.log(paddingDays);
    document.getElementById('monthDisplay').innerText =
        `${dt.toLocaleDateString('en-us', { month: 'long' })} ${year}`; //id monthDisplay comes from the html page CalendarInex

    calendar.innerHTML = ''; //reset the calender so when loading a new calender they wont stack untop eachother in html page
    //add daySquare to calender
    for (let i = 1; i <= daysInMonth; i++) { //we need to render emty suares on the screen so which include paddingDays so (paddingDays + daysInMonth)
        const daySquare = document.createElement('div'); //for every iteration we are gonna create a div and asign it to daySquare
        daySquare.classList.add('day'); //give daySquare a class called day
        console.log(i);
        const dayString = `${month + 1}/${i}/${year}`; //string sending to openModal function. subtraction paddingDays from i becouse we dont want them in


        daySquare.innerText = i; //get number for current square we are on

        const eventForDay = events.find(e => e.date === dayString);

        //show current day with green glow on calender
        if (i === day) { // nav === 0 ao we only hightligt current day for current month not all months
            daySquare.id = 'currentDay';

        }

        if (eventForDay) { //if there is a event for the day we are on
            const eventDiv = document.createElement('div'); // create a div
            eventDiv.classList.add('event'); //add a class
            eventDiv.innerText = eventForDay.title; //set the text
            daySquare.appendChild(eventDiv); //and put it inside a day square
        }


        daySquare.addEventListener('click', () => openModal(dayString));//want to run a function everytime a user clicks on it



        //

        calendar.appendChild(daySquare);
    }
}


function closeModal() {
    eventTitleInput.classList.remove('error'); //removes error when closing modal
    newEventModal.style.display = 'none'; //cloce given modal (pop-up vindoe)
    deleteEventModal.style.display = 'none'; //cloce given modal (pop-up vindoe)
    backDrop.style.display = 'none';

    eventTitleInput.value = ''; //clear the textbox when closing modal
    clicked = null; //clear clicked so its null at beginning next time something is clicked

    load();
}

function saveEvent() {
    if (eventTitleInput.value) { //if user has typed some value save event 
        eventTitleInput.classList.remove('error'); //removes error like from previus attempt

        //puts the input value into list/array //pushing object into list/array
        //events.push({ 
        //    date: clicked,
        //    title: eventTitleInput.value

        //});

        //Send the JSON array to Controller using AJAX.
        var data = {

            date: clicked.toString(), //can only send string through ajax
            title: eventTitleInput.value
            
        }

        console.log('Submitting form...');
        $.ajax({
            type: 'POST',
            url: '/Bookings/FaellesKaekken',
            dataType: 'json',

            data: data,

            success: function (result) {
                console.log('Data received: ');
                console.log(result);
            }
        });


        


        //localStorage.setItem('events', JSON.stringify(events)); //we have to use JSON.stringify() since we cant store a object in local storage

    } else { //else give error
        eventTitleInput.classList.add('error');

    }
}


function deleteEvent() {
    events = events.filter(e => e.date !== clicked); //gets all the dates in storage that is not the current modal(clicked day square)
    localStorage.setItem('events', JSON.stringify(events)); //and therefor the date the user was on will not be saved in local storage only the other dates in events array/lst will
    //Send the JSON array to Controller using AJAX.
    //Send the JSON array to Controller using AJAX.
    var data = {

        date: clicked.toString(), //can only send string through ajax
        Calendar: "Kaekken"
    }


    console.log('Submitting form...');
    $.ajax({
        type: 'DELETE',
        url: '/Bookings/DeleteConfirmedEvent',
        dataType: 'json',

        data: data,

        success: function (result) {
            console.log('Data received: ');
            console.log(result);
        }
    });

    localStorage.removeItem(clicked.toString())

    localStorage.clear();


}

function initButton() {

    document.getElementById('saveButton').addEventListener('click', () => {

        saveEvent();
        closeModal();
        setTimeout(() => window.location.reload(), 600);//wait 600ms
    });

    document.getElementById('cancelButton').addEventListener('click', () => {

        closeModal();
    });

    document.getElementById('deleteButton').addEventListener('click', () => {
        deleteEvent();

        closeModal();
        setTimeout(() => window.location.reload(), 600);//wait 600ms
    });
    document.getElementById('closeButton').addEventListener('click', () => {



        closeModal();
    });
}


initButton();
load();

