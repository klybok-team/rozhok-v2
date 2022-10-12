const fs = require('fs');

const axios = require('axios');

module.exports = async(url, path) => {
    const res = await axios.get(url, {
        responseType: 'arraybuffer',
    })

    if(res['content-type'] === 'image/jpg') {
        res['content-type'] = 'image/jpeg'
    };

    return Buffer.from(res.data, 'utf8')
};
