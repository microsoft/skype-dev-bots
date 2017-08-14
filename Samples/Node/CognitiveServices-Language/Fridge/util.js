// helper functions

var isFirstTime = function (session) {
    return (session.userData.ingredients == null);
};

var initFridge = function (session) {
    if (isFirstTime(session)) {
        session.userData.ingredients = [];
    }
};

var isInFridge = function (session, item) {
    return isFirstTime(session) ? false : (session.userData.ingredients.indexOf(item) != -1);
};

var addToFridge = function (session, item) {
    if (isFirstTime(session)) {
        session.userData.ingredients = [item];
    } else {
        session.userData.ingredients.push(item);
    }
};

var removeFromFridge = function (session, item) {
    if (isFirstTime(session)) {
        return false;
    }
    
    var idx = session.userData.ingredients.indexOf(item);
    var removed = session.userData.ingredients.splice(idx, 1);
    
    // if splice returns an empty array, there is an error
    if (removed.length != 1) {
        return false;
    }
    return true;
};

var removeAllFromFridge = function (session) {
    session.userData.ingredients = [];
}

var itemsToString = function (items) {
    if (typeof items == 'undefined' || items.length == 0) {
        return 'nothing';
    }

    var itemsString = '';
    for (var idx = 0; idx < items.length; idx++) {
        itemsString += items[idx] + ', ';
    }

    return itemsString.slice(0, -2);
}

module.exports = {
    // checks whether the user accessed the bot for the first time
    isFirstTime: isFirstTime,

    // initializes userdata for ingredients
    initFridge: initFridge,

    // checks whether the given item is in the fridge
    isInFridge: isInFridge,

    // adds given item to the fridge
    addToFridge: addToFridge,

    // removes given item from the fridge
    removeFromFridge: removeFromFridge,

    // removes all items from the fridge
    removeAllFromFridge: removeAllFromFridge,

    // given an array of items, format them in a user-friendly way
    itemsToString: itemsToString 
};