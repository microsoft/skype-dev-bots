'use strict';
const strings = require('../Strings');
const store = require('../store');
const builder = require('botbuilder');
const noteUtil = require('../util/NoteUtil');
const util = require('util')

const connector = new builder.ChatConnector({
  appId: process.env.MICROSOFT_APP_ID,
  appPassword: process.env.MICROSOFT_APP_PASSWORD
});

const bot = new builder.UniversalBot(connector, [
  (session, args, next) => {
    session.replaceDialog('Help', {action: 'unknown'});
  },
]);

bot.on('contactRelationUpdate', message => {
  if (message.action === 'add') {
    const name =  message.user.name || + '(Id: ' + m.id + ')';
    bot.beginDialog(message.address, 'Intro', { user: name });
  }
});

bot.dialog('Intro', [
  (session, args, next) => {
    const card = getIntroCard(session, args.user);
    const message = new builder.Message(session);
    message.addAttachment(card);
    session.endDialog(message);
  }
]);

bot.dialog('Note', [
  (session, args, next) => {
    const userId = session.message.address.user.id;
    const content = session.message.text.slice(5);
    if (!content) {
      session.endDialog(strings.NOTE_NO_INPUT);
    } else {
      store.saveNote(userId, content, (error, result) => {
        if (error) {
          session.endDialog(strings.NOTE_CANNOT_SAVE);
        } else {
          const card = getSavedNoteCard(session, result);
          const msg = new builder.Message(session).addAttachment(card);
          session.endDialog(msg);
        }
      });
    }
  }
]).triggerAction({
    matches: [/^note$/i, /note\W/i]
  });

bot.dialog('Show', [
  (session, results, next) => {
    const userId = session.message.address.user.id;
    const searchText = session.message.text.slice(5);

    store.getNotes(userId, searchText, notes => {
      const message = new builder.Message(session)
        .text(getShowNotesString(notes, session.message.localTimestamp))
        .textFormat("xml");
      session.endDialog(message);
    });
  }
]).triggerAction({
    matches: [/^show$/i, /show\W/i]
  });

bot.dialog('Delete', [
  (session, args, next) => {
    const card = getDeleteCard(session);
    // attach the card to the reply message
    const msg = new builder.Message(session).addAttachment(card);
    session.send(msg);
  }
]).triggerAction({
    matches: /^delete$/i
  });

bot.dialog('Delete force', [
  (session, args, next) => {
    const userId = session.message.user.id;
    store.deleteAllNotes(userId, deletedCount => {
      let message = '';
      if (deletedCount > 0) {
        message  = util.format(strings.DELETE_FORCE_SUCC, deletedCount);
      } else {
        message = strings.DELETE_FORCE_FAIL;
      }
      session.endDialog(message);
    })
  }
]).triggerAction({
    matches: /^delete force$/i
  });

bot.dialog('Help', [
  (session, args, next) => {
    let message = '';
    const action = args.action ? args.action.toLowerCase() : null;
    switch (action) {
      case 'note':
        message = strings.HELP_NOTE;
        break;
      case 'show':
        message = strings.HELP_SHOW;
        break;
      case 'delete':
        message = strings.HELP_DELETE;
        break;
      case 'help':
        // Fall-through
      default:
        const explicitHelp = action === 'help';
        let card = getHelpCard(session, explicitHelp);
        message = new builder.Message(session).addAttachment(card);
        break;
    }
    session.endDialog(message);
  }
]).triggerAction({
    matches: /^help/i,
    onSelectAction: (session, args) => {
      const action = args.action;
      const subHelp = args.intent.matched.input.slice(5);
      args.action = subHelp ? subHelp : 'help';
      session.beginDialog(action, args);
    }
  });

function getHelpCard(session, explicitHelp) {
  return new builder.HeroCard()
    .title(explicitHelp ? strings.HELP_CARD_TITLE_EXPLICIT : strings.HELP_CARD_TITLE_IMPLICIT)
    .text(strings.HELP_CARD_TEXT)
    .buttons([
      new builder.CardAction(session).title(strings.NOTE).value('help note').type('imBack'),
      new builder.CardAction(session).title(strings.SHOW).value('help show').type('imBack'),
      new builder.CardAction(session).title(strings.DELETE).value('help delete').type('imBack'),
    ]);
}

function getDeleteCard(session) {
  return new builder.HeroCard()
    .title(strings.DELETE_CARD_TITLE)
    .text(strings.DELETE_CARD_TEXT)
    .buttons([
      new builder.CardAction(session).title(strings.DELETE_CARD_CHOICE_YES).value('delete force').type('imBack'),
      new builder.CardAction(session).title(strings.DELETE_CARD_CHOICE_NO).value('show').type('imBack'),
    ]);
}

function getSavedNoteCard(session, note) {
  return new builder.HeroCard()
    .title(strings.SAVED_NOTE_CARD_TITLE)
    .subtitle(note.Content)
    .buttons([
      new builder.CardAction(session).title(strings.SAVED_NOTE_CARD_CHOICE_SHOW).value('show').type('imBack'),
      new builder.CardAction(session).title(strings.SAVED_NOTE_CARD_CHOICE_DELETE).value('delete').type('imBack'),
    ]);
}

function getShowNotesString(notes, localTimestamp) {
  const noteCount = notes ? notes.length : 0;
  const title = util.format(strings.SHOW_NOTES_CARD_TITLE, noteCount);
  
  let text = "";
  text += title;

  for (let i = 0; i < noteCount; i++) {
    let note = notes[i];
    text += '\n\n<i>' + noteUtil.formatDate(noteUtil.ticksToDate(note.Timestamp), localTimestamp) + '</i>';
    text += '\n' + note.Content;
  }

  return text;
}

function getIntroCard(session, user) {
  const card = new builder.HeroCard(session);
  card.title(util.format(strings.INTRO_CARD_TITLE, user))
  .buttons([
    new builder.CardAction(session).title(strings.HELP_CARD_TITLE_EXPLICIT).value('help').type('imBack'),
  ])
  .text(strings.INTRO_CARD_TEXT)
  .images([
    new builder.CardImage(session).url(strings.WELCOME_IMAGE_URL)
  ]);

  return card;
}

module.exports = bot;