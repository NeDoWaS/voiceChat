let localStream = null;

export async function startMicrophone() {
    try {
        localStream = await navigator.mediaDevices.getUserMedia({
            audio: true,
            video: false
        });

        console.log("Microphone initialized");

        return true;
    }
    catch (error) {
        console.error("Microphone error:", error);
        throw error;
    }
}

export function stopMicrophone() {
    if (localStream) {
        localStream.getTracks().forEach(track => track.stop());
        localStream = null;
    }
}