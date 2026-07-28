import styles from '../title_video/title_video.module.css';

function TitleVideo({title}){
    return(
        <p className={styles.title_video}>{title}</p>
    )
}

export default TitleVideo;