const downloadFile = require('../functions/downloadFile.js')
const demotivatorImage = require('../functions/demotivatorImage.js');

module.exports = {
	name: 'help',
	aliases: ['h'],
    desc: 'Узнать список команд',
	async execute(client, m, args) {
        if(!args) return client.createMessage(m.channel.id, `Вот список доступных вам команд: ${client.cmd.map(obj => obj.name).join(', ')}`)

        
        args = args.split(' ')

        let txt;

        for(obj of client.cmd.map(m => m)) {
            if(obj.name === args[0]) txt = obj.desc

        }
        if(!txt) return client.createMessage(m.channel.id, 'Такой команды нет.')
        
        return client.createMessage(m.channel.id, txt)
	}
};