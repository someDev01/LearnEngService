import styles from '../duration/duration.module.css';

function Duration({hours, minutes, seconds}){

    const formatTime = (time) => time.toString().padStart(2, '0');

    const timeString = hours > 0 
        ? `${hours}:${formatTime(minutes)}:${formatTime(seconds)}`
        : `${minutes}:${formatTime(seconds)}`;

    return(
        <div className={styles.duration}>
            <p>{timeString}</p>
        </div>
    )
}

export default Duration;