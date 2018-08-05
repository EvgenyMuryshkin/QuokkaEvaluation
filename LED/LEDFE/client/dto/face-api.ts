export interface IFaceAPIResponse {
    faceAttributes: {
        emotion: {
            anger: number,
            contempt: number,
            disgust: number,
            fear: number,
            happiness: number,
            neutral: number,
            sadness: number,
            surprise: number
        }
        age: number,
    },
    faceRectangle: {
        top: number,
        left: number,
        bottom: number,
        right: number
    }
}