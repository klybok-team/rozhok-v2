const downloadFile = require('../functions/downloadFile.js')
const demotivatorImage = require('../functions/demotivatorImage.js');

module.exports = {
	name: 'help',
	aliases: ['h'],
    desc: 'Узнать список команд',
	async execute(client, m, args, areYouDev) {

        let normalCommands = []
        let devCommands = []

        let map = client.cmd.map(m => m)

        for(let obj of map) {
                if(obj.devAccess) {
                    if(areYouDev === true) {
                        devCommands.push(obj.name)
                    }
                }
                if(!obj.devAccess) {
                    normalCommands.push(obj.name)
                }
        }
        
        if(!args) {
            let msg = `Привет! Я рожок.\nВот список доступных вам команд: ${normalCommands.join(', ')}`
            if(devCommands.length > 0) msg = msg + `\n\nПОЛУЧЕН ДОСТУП К КАРТОЧКЕ DEV... СПИСОК ДОСТУПНЫХ ВАМ КОМАНД: ${devCommands.join(', ')}`
            if(!args) return client.createMessage(m.channel.id, msg)
    
        };
        
        args = args.split(' ')

        let txt;

        for(obj of client.cmd.map(m => m)) {
            if(obj.name === args[0]) txt = obj.desc

        }
        if(!txt) return client.createMessage(m.channel.id, 'Такой команды нет.')
        
        return client.createMessage(m.channel.id, txt)
	}
};