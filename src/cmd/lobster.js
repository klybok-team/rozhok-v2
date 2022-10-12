const fs = require('fs')
const prikol = require('../functions/prikol.js');
const { lobsterReplies, other } = require('../../config.js')
const downloadFile = require('../functions/downloadFile.js')

module.exports = {
	name: 'lobster',
	aliases: ['l'],
	async execute(client, m, args) {
		if(!args) return client.createMessage(m.channel.id, lobsterReplies.ErrorArgs.noText);

        if(args.lenght > 99) return client.createMessage(m.channel.id, lobsterReplies.Errorlenght);

		let attachment;

		if(!m.attachments[0]) {
			if(m.messageReference?.messageID && m.messageReference?.channelID) {
				let messageReferenceAttachments = await client.getMessage(m.messageReference.channelID, m.messageReference.messageID)

				attachment = messageReferenceAttachments?.attachments[0];
			};
		};
		if(m.attachments[0]) attachment = m.attachments[0];
	
		if(!attachment) return client.createMessage(m.channel.id, lobsterReplies.ErrorArgs.noImg);
		
		if(attachment.filename.endsWith('.jpg') || attachment.filename.endsWith('.jpeg') || attachment.filename.endsWith('.png')) {
            return await downloadFile(`${attachment.url}`).then(async source => {

            let image = await prikol(args, 'textOnImg', source);

            client.createMessage(m.channel.id, {}, [{ file: image, name: attachment.filename }]);
		    });
	    } 
	return client.createMessage(m.channel.id, other.fileExNotNed);
	}
};