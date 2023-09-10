export class SecureService {
    keys: { privateKey: string; publicKey: string; } | null = null;
    vector = crypto.getRandomValues(new Uint8Array(16))
    encryptAlgorithm = {
        name: "RSA-OAEP",
        modulusLength: 2048,
        publicExponent: new Uint8Array([1, 0, 1]),
        extractable: false,
        hash: {
          name: "SHA-256"
        }
      }

    public async create() : Promise<any> {
        if (this.keys) {
            return
        }
        const criptoKeys = await crypto.subtle.generateKey(this.encryptAlgorithm, true, ["encrypt", "decrypt"]) as CryptoKeyPair
        const publicKey = this.convertBinaryToPem(await crypto.subtle.exportKey('spki', criptoKeys.publicKey), "RSA PUBLIC KEY");
        const privateKey = this.convertBinaryToPem(await crypto.subtle.exportKey('pkcs8', criptoKeys.privateKey), "RSA PRIVATE KEY");
        this.keys = {
            privateKey,
            publicKey
        };
    }

    public getPublicKey() : string {
        if (!this.keys) {
            throw new Error("create")
        }
        return this.keys.publicKey;
    }

    public getPrivateKey() : string {
        if (!this.keys) {
            throw new Error("create")
        }
        return this.keys.privateKey;
    }

    public async encrypt(publicKey: string, value: string) : Promise<string> {
        const criptoPublicKey = await this.importPublicKey(publicKey);
        const data = await crypto.subtle.encrypt({
            name: "RSA-OAEP",
            iv: this.vector
        }, criptoPublicKey, this.textToArrayBuffer(value));
        return this.arrayBufferToBase64String(data);
    }

    public async decrypt(encrypted: string) : Promise<string> {
        const data = this.base64StringToArrayBuffer(encrypted)
        const privateKey = this.getPrivateKey()
        const criptoPrivateKey = await this.importPrivateKey(privateKey);
        const arr = await crypto.subtle.decrypt({
            name: "RSA-OAEP",
            iv: this.vector
        }, criptoPrivateKey, data)
        const text = this.arrayBufferToText(arr)
        return text;
    }

    async importPrivateKey(pemKey: string) {
        return await crypto.subtle.importKey("pkcs8", this.convertPemToBinary(pemKey), this.encryptAlgorithm, true, ["decrypt"])
    }

    async importPublicKey(pemKey: string) {
        return await crypto.subtle.importKey("spki", this.convertPemToBinary(pemKey), this.encryptAlgorithm, true, ["encrypt"])
    }

    convertBinaryToPem(binaryData: ArrayBuffer, label: string) {
        var base64Cert = this.arrayBufferToBase64String(binaryData)
        var pemCert = "-----BEGIN " + label + "-----\r\n"
        var nextIndex = 0
        while (nextIndex < base64Cert.length) {
            if (nextIndex + 64 <= base64Cert.length) {
                pemCert += base64Cert.substr(nextIndex, 64) + "\r\n"
            } else {
                pemCert += base64Cert.substr(nextIndex) + "\r\n"
            }
            nextIndex += 64
        }
        pemCert += "-----END " + label + "-----\r\n"
        return pemCert
    }

      arrayBufferToBase64String(arrayBuffer: ArrayBuffer) {
        var byteArray = new Uint8Array(arrayBuffer)
        var byteString = ''
        for (var i=0; i<byteArray.byteLength; i++) {
          byteString += String.fromCharCode(byteArray[i])
        }
        return btoa(byteString)
      }

      convertPemToBinary(pem: string) {
        var lines = pem.split('\n')
        var encoded = ''
        for(var i = 0;i < lines.length;i++){
          if (lines[i].trim().length > 0 &&
              lines[i].indexOf('-BEGIN RSA PRIVATE KEY-') < 0 &&
              lines[i].indexOf('-BEGIN RSA PUBLIC KEY-') < 0 &&
              lines[i].indexOf('-END RSA PRIVATE KEY-') < 0 &&
              lines[i].indexOf('-END RSA PUBLIC KEY-') < 0) {
            encoded += lines[i].trim()
          }
        }
        return this.base64StringToArrayBuffer(encoded)
      }

      base64StringToArrayBuffer(b64str: string) {
        var byteStr = atob(b64str)
        var bytes = new Uint8Array(byteStr.length)
        for (var i = 0; i < byteStr.length; i++) {
          bytes[i] = byteStr.charCodeAt(i)
        }
        return bytes.buffer
      }

      textToArrayBuffer(str: string) {
        var buf = decodeURI(str)
        var bufView = new Uint8Array(buf.length)
        for (var i=0; i < buf.length; i++) {
          bufView[i] = buf.charCodeAt(i)
        }
        return bufView
      }

      arrayBufferToText(arrayBuffer: ArrayBuffer) {
        var byteArray = new Uint8Array(arrayBuffer)
        var str = ''
        for (var i=0; i<byteArray.byteLength; i++) {
          str += String.fromCharCode(byteArray[i])
        }
        return str
      }
 }