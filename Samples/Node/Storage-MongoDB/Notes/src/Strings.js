module.exports = Object.freeze({
  WELCOME_IMAGE_URL: "https://notesbotapp.azurewebsites.net/Resources/welcome_card.png",

  NOTE: "note",
  SHOW: "show",
  DELETE: "delete",
  HELP: "help",

  NOTE_NO_INPUT: "Please add text to save the note.",
  NOTE_CANNOT_SAVE: "Cannot save note.",

  DELETE_FORCE_SUCC: "All of your **%d notes** have been **permanently deleted.**",
  DELETE_FORCE_FAIL: "You have no notes to delete.",

  HELP_NOTE: "Use the **note** command to capture a simple note. For example... \n\n_note call mom tomorrow_\n\n_note send the project proposal_",
  HELP_SHOW: "Use the **show** command to display all of your notes. You can also display only notes that contain specific words."
    + " For example...\n\n_show_\n\n_show call mom_\n\n_show project_",
  HELP_DELETE: "Use the **delete** command to delete all your notes. You will be prompted before final deletion.",

  HELP_CARD_TITLE_EXPLICIT: "Help",
  HELP_CARD_TITLE_IMPLICIT: "Sorry, I didn\'t get that.",
  HELP_CARD_TEXT: "Select a command to learn how to use it.",

  DELETE_CARD_TITLE: "Delete all notes?",
  DELETE_CARD_TEXT: "This will permanently delete all of your notes. Are you sure?",
  DELETE_CARD_CHOICE_YES: "Yes, delete all",
  DELETE_CARD_CHOICE_NO: "No, show all",

  SAVED_NOTE_CARD_TITLE: "Note added successfully!",
  SAVED_NOTE_CARD_CHOICE_SHOW: "Show notes",
  SAVED_NOTE_CARD_CHOICE_DELETE: "Delete notes",
  SAVED_NOTE_CARD_NO_TAGS: "No tags found",
  SHOW_NOTES_CARD_TITLE: "You have <b>%d notes.</b>",

  INTRO_CARD_TITLE: "Hello %s, I am the Notes bot.",
  INTRO_CARD_TEXT: "I am here to be your assistant and take notes for you."
});
