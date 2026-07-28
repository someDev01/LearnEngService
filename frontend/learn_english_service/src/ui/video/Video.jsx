import { useDispatch } from 'react-redux';
import Duration from '../duration/Duration';
import LexicalComplexity from '../lexical_complexity/LexicalComplexity';
import PreviewVideo from '../preview_video/PreviewVideo';
import styles from '../video/video.module.css';
import { openVideoModal } from '../../redux/slices/modalSlice';
import TitleVideo from '../title_video/TitleVideo';
import { closeMenu } from '../../redux/slices/menuSlice';
import { useNavigate } from 'react-router-dom';

function Video({video, youtubeId, setOpenedVideo, onNavigate}){

    const dispatch = useDispatch();  
    
    const onCloseMenu = () => {dispatch(closeMenu())};
    return(
        <>
            <div className={styles.video} onClick={() => {
                setOpenedVideo(video);
                onNavigate();
                onCloseMenu();
            }}>
                <div className={styles.video_preview}>
                    <PreviewVideo imgUrl={`https://img.youtube.com/vi/${youtubeId}/mqdefault.jpg`}/> 
                    <Duration 
                        hours={video.duration.hours ?? 0}
                        minutes={video.duration.minutes ?? 0}
                        seconds={video.duration.seconds ?? 0}
                    />
                </div>
                <div className={styles.details}>
                    <TitleVideo title={video.titleVideo}/>
                    <LexicalComplexity video={video}/>
                </div>
            </div>
        </>
    )
}

export default Video;