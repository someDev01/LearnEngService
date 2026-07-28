import BackButton from '../../../../shared/ui/back_button/BackButton';
import LexicalComplexity from '../../../../ui/lexical_complexity/LexicalComplexity';
import InfoVideo from '../info_video/InfoVideo';
import styles from '../video_player_header/video_player_header.module.css';

function VideoPlayerHeader({video}){
    return(
        <header className={styles.video_player_header}>
            <BackButton/>
            <InfoVideo title={title}/>
        </header>
    )
}

export default VideoPlayerHeader;