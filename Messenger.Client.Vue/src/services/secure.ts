import NodeRSA from 'node-rsa';

export class SecureService {
    key = new NodeRSA({b: 512});

    public getPublicKey() {
        return this.key.exportKey("public");
    }

    public encrypt(publicKey: string, value: string) : string {
        const tempKey = new NodeRSA();
        tempKey.importKey(publicKey, "public")
        return tempKey.encrypt(value, "base64")
    }

    public decrypt(encrypted: string) : string {
        return this.key.decrypt(encrypted, 'utf8');
    }
 }