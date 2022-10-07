const fs = require('fs')
const prikol = require('../functions/prikol.js');
const { lobsterReplies, waitTime, other } = require('../../config.js')
const downloadFile = require('../functions/downloadFile.js')

module.exports = {
	name: 'demotivator',
	aliases: ['d', 'dem'],
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
            let path = `./temp/${m.id}_${attachment.filename}`;
            await downloadFile(`${attachment.url}`, `./temp/${m.id}_${attachment.filename.replace('.jpg', '.jpeg')}`);
    
            const sleep = require('util').promisify(setTimeout);
            await sleep(waitTime);

            let image = await prikol(args, 'textOnImg', fs.readFileSync(path));
            return client.createMessage(m.channel.id, {}, [{ file: image, name: attachment.filename }]);
        };
        return client.createMessage(m.channel.id, other.fileExNotNed);
    }
};