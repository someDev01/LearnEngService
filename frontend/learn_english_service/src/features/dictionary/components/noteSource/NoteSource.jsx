import { useDispatch } from 'react-redux';
import { closeModalViewNote } from '../../../../redux/slices/modalSlice';
import Source from '../../../../ui/source/Source';
import styles from '../noteSource/note_source.module.css';
import { useNavigate } from 'react-router-dom';
import useEmblaCarousel from "embla-carousel-react";

function NoteSource({source, word}){

    const dispatch = useDispatch();
    const navigation = useNavigate();

    const [emblaRef] = useEmblaCarousel({
        loop:false,
        align:'start',
        slidesToScroll:1
    });

    const getStartTime = (duration) => {
        if(!duration) return 0;

        return (duration.hours || 0) * 3600 + 
                (duration.minutes || 0) * 60 +
                (duration.seconds || 0);
    }

    const onNavigationToPlayer = (video) => {
            dispatch(closeModalViewNote());
            navigation("/video-player", {
                state:{
                    video
                }
            })
        }

    return(
         <div className={styles.source_block}>
            <p style={{margin: 0, color: '#6c5a51'}}>Источники ({source.length})</p>

            <div className={styles.viewport} ref={emblaRef}>
                <div className={styles.container}>
                    {source.map((item, index) => {
                        const video = {
                            id: item.youtubeVideoId,
                            youtubeId: item.youtubeId,
                            titleVideo: item.titleVideo,
                            startTime: getStartTime(item.durationContext)
                        }

                        return(
                            <div className={styles.slide} key={item.id ?? index}>
                                <Source
                                    youtubeVideoTitle={item.youtubeVideoTitle}
                                    context={item.context}
                                    duration={item.durationContext}
                                    word={word}
                                    onNavigateToPlayer={() => onNavigationToPlayer(video)}
                                />
                            </div>
                        )
                    })}
                </div>
            </div>
        </div>
    )
}

export default NoteSource;