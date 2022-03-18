// List of sentences
var _CONTENT = ["Searching for selected documentation...", "Documentation is being prepared...", "Adding little details...", "Documentation almost ready... Please wait..."];

// Current sentence being processed
var _PART = 0;

// Character number of the current sentence being processed 
var _PART_INDEX = 0;

// Holds the handle returned from setInterval
var _INTERVAL_VAL;

// Element that holds the text
var _ELEMENT = document.querySelector("#loading");


// Implements typing effect
function Type() {
    var text = _CONTENT[_PART].substring(0, _PART_INDEX + 1);
    _ELEMENT.innerHTML = text;
    _PART_INDEX++;

    // If full sentence has been displayed then start to delete the sentence after some time
    if (text === _CONTENT[_PART]) {
        clearInterval(_INTERVAL_VAL);
        setTimeout(function () {
            _INTERVAL_VAL = setInterval(Delete, 30);
        }, 600);
    }
}
function Keyboard() {
    _ELEMENT.classList.toggle("keyboard-effect");
}

// Implements deleting effect
function Delete() {
    if (_PART < 3) {
        var text = _CONTENT[_PART].substring(0, _PART_INDEX - 1);
        _ELEMENT.innerHTML = text;
        _PART_INDEX--;

        // If sentence has been deleted then start to display the next sentence
        if (text === '') {
            clearInterval(_INTERVAL_VAL);

            // If last sentence then display the first one, else move to the next
            if (_PART != (_CONTENT.length - 1))
                _PART++;


            // Start to display the next sentence after some time
            setTimeout(function () {
                _INTERVAL_VAL = setInterval(Type, 50);
            }, 100);
        }
    }

}

// Start the typing effect on load
_INTERVAL_VAL = setInterval(Type, 80);